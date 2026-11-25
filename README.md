# DSW_3P_PR01 – Gestor de Eventos Tecnológicos

API REST (.NET 8, ASP.NET Core Web API) para administrar eventos tecnológicos (hackatones, conferencias y talleres).
Arquitectura: Controller (MVC) → Service → IEventoRepository → Repository concreto → almacenamiento.

## Modelo Evento
Id, Nombre, Categoria, Fecha, Lugar, CupoMaximo, Activo

## Validaciones
- Nombre obligatorio
- CupoMaximo mayor que cero
- Fecha válida (no vacía)

Responden `400 Bad Request` con el mensaje de error.

## Endpoints
| Método | Ruta | Descripción |
|--------|------|-------------|
| POST | /api/auth/token | Obtiene un token JWT válido por 30 minutos usando la llave de acceso |
| GET | /api/eventos | Lista todos (requiere token) |
| GET | /api/eventos/{id} | Obtiene por Id (requiere token) |
| POST | /api/eventos | Crea (requiere token) |
| PUT | /api/eventos/{id} | Actualiza (requiere token) |
| DELETE | /api/eventos/{id} | Elimina (requiere token) |

Swagger: http://localhost:5080/swagger

## Ejemplo de clientes de la API

### Swagger
1. Abre `http://localhost:5080/swagger`.
2. Ejecuta `POST /api/auth/token`.
3. Copia el campo `accessToken`.
4. Pulsa **Authorize**, escribe `Bearer {accessToken}` y prueba los endpoints protegidos.

### Postman
Importa la colección [postman/DSW_3P_PR01.postman_collection.json](postman/DSW_3P_PR01.postman_collection.json)
y el entorno [postman/DSW_3P_PR01.postman_environment.json](postman/DSW_3P_PR01.postman_environment.json).
El entorno usa `http://localhost:5080` como `baseUrl` y contiene `apiKey` como variable.

1. Selecciona el entorno.
2. Configura `apiKey` con el valor de `Authentication:ApiKey`.
3. Ejecuta **Auth / Obtener token**. El script de prueba guarda `accessToken` automáticamente.
4. Ejecuta las solicitudes de **Eventos**; todas heredan el Bearer token.

## Autenticación
Los endpoints de eventos requieren un token Bearer. La llave de acceso sirve para
solicitar un token y no se envía en cada operación. El token vence a los 30 minutos;
al vencer, solicita uno nuevo. Para desarrollo local, las llaves ya están definidas
en `appsettings.json`, así que puedes iniciar con `dotnet run` sin configurar variables
de entorno. El inicio de sesión de demostración usa el usuario `admin` y la contraseña
`questlog123`; el endpoint `POST /api/auth/token` usa la ApiKey local de desarrollo
definida en `appsettings.json`.

Las llaves incluidas son únicamente para desarrollo y no deben usarse en producción.
En un entorno real, reemplázalas mediante variables de entorno:

```powershell
$env:Authentication__ApiKey = [Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
$env:Authentication__SigningKey = [Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
dotnet run
```

Guarda ambos valores generados para usarlos en las solicitudes. La llave de firma
debe tener al menos 32 bytes.

Solicitar un token:
```powershell
$tokenResponse = Invoke-RestMethod -Method Post `
  -Uri http://localhost:5080/api/auth/token `
  -ContentType "application/json" `
  -Body (@{ apiKey = $env:Authentication__ApiKey } | ConvertTo-Json)
$token = $tokenResponse.accessToken
```

Usar el token, por ejemplo:
```powershell
Invoke-RestMethod -Uri http://localhost:5080/api/eventos `
  -Headers @{ Authorization = "Bearer $token" }
```

En Swagger, pulsa **Authorize** e ingresa `Bearer <token>` para probar los endpoints
protegidos. La llave de acceso es una credencial fija configurada por el operador;
lo que vence y debe volver a solicitarse cada 30 minutos es el token JWT.

## Historial de versiones
1. `chore: proyecto inicial y modelo Evento`
2. `feat: CRUD de eventos con Repository, Service y persistencia JSON (Swagger)`  ← versión JSON
3. `feat: integrar EF Core y repositorio SQL Server` (rama `feature/sql-server`)
4. `docs: README con endpoints, migraciones y publicación`
5. Migración `Inicial` y creación de la tabla `Eventos` mediante EF Core.

## Ejecución
```bash
dotnet restore
dotnet run
```

## Migraciones (versión SQL Server)
La cadena de conexión predeterminada usa SQL Server Express local (`.\SQLEXPRESS`).
Modifica `ConnectionStrings:DefaultConnection` en `appsettings.json` si tu instancia es diferente.

```bash
dotnet ef migrations add Inicial
dotnet ef database update
```

La migración `Inicial` crea la tabla `Eventos` con las siete propiedades del modelo.
La interfaz `IEventoRepository`, el Controller y el Service se conservan; el cambio
de persistencia se concentra en `EventoSqlRepository`, `AppDbContext`, la configuración
de Entity Framework Core y el registro de dependencias en `Program.cs`. `EventoJsonRepository`
permanece como implementación de la versión anterior.

Verificar en SQL Server / SSMS:
```sql
USE EventosDb;
SELECT * FROM Eventos;
```

## Ejemplo de cuerpo para POST
```json
{
  "nombre": "Salvar a Poncho",
  "categoria": "Misión",
  "fecha": "2026-12-20T10:00:00",
  "lugar": "Aldea de los Exploradores",
  "cupoMaximo": 10,
  "dificultad": "Difícil",
  "recompensaXP": 100000,
  "activo": true
}
```

Casos inválidos para probar: `nombre` vacío, `cupoMaximo` 0 o negativo, `fecha` omitida.

## Publicar en GitHub
Esta entrega se publicó en la rama `eventos-tecnologicos` para no sobrescribir
`main`, `master` ni `feature/sql-server`, que ya contenían otra aplicación.

Repositorio: https://github.com/oscaritoag61-cmyk/DSW_3P_PR01/tree/eventos-tecnologicos

Para descargar esta versión:
```bash
git fetch origin eventos-tecnologicos
git switch --track -c eventos-tecnologicos origin/eventos-tecnologicos
```
