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
| GET | /api/eventos | Lista todos |
| GET | /api/eventos/{id} | Obtiene por Id |
| POST | /api/eventos | Crea |
| PUT | /api/eventos/{id} | Actualiza |
| DELETE | /api/eventos/{id} | Elimina |

Swagger: http://localhost:5080/swagger

## Historial de versiones
1. `chore: proyecto inicial y modelo Evento`
2. `feat: CRUD de eventos con Repository, Service y persistencia JSON (Swagger)`  ← versión JSON
3. `feat: integrar EF Core y repositorio SQL Server` (rama `feature/sql-server`)
4. `feat: migración inicial de la tabla Eventos` ← se crea al ejecutar las migraciones

## Ejecución
```bash
dotnet restore
dotnet run
```

## Migraciones (solo versión SQL Server)
Ajusta la cadena de conexión en `appsettings.json` si no usas LocalDB
(por ejemplo SQL Server Express: `Server=.\\SQLEXPRESS;Database=EventosDb;Trusted_Connection=True;TrustServerCertificate=True;`).

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add Inicial
dotnet ef database update
```

Verificar en SQL Server / SSMS:
```sql
USE EventosDb;
SELECT * FROM Eventos;
```

## Ejemplo de cuerpo para POST
```json
{
  "nombre": "Conferencia de Inteligencia Artificial",
  "categoria": "Conferencia",
  "fecha": "2026-12-20T10:00:00",
  "lugar": "Auditorio Principal",
  "cupoMaximo": 200,
  "activo": true
}
```

Casos inválidos para probar: `nombre` vacío, `cupoMaximo` 0 o negativo, `fecha` omitida.

## Publicar en GitHub
```bash
git remote add origin URL_DEL_REPOSITORIO
git switch main
git merge feature/sql-server
git push -u origin main
git push origin feature/sql-server
```
