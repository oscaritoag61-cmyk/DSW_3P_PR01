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
