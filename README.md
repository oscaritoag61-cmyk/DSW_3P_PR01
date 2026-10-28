# QuestLog – Sistema de Gestión de Misiones

API REST (ASP.NET Core 8, controladores MVC) para administrar las misiones de un videojuego.
Arquitectura en capas: **Controller → Service → Repository**.

**Etapa 1 (este commit):** persistencia en archivo JSON (`Data/misiones.json`).
**Etapa 2 (siguiente):** SQL Server con Entity Framework Core, cambiando solo el repositorio.

## Estructura

```
QuestLog/
├── QuestLog.sln
└── QuestLog/
    ├── Controllers/MisionesController.cs   # HTTP: rutas y códigos de estado
    ├── Services/
    │   ├── IMisionService.cs
    │   ├── MisionService.cs                # reglas de negocio
    │   └── Excepciones.cs
    ├── Repositories/
    │   ├── IMisionRepository.cs            # contrato de persistencia
    │   └── MisionJsonRepository.cs         # implementación JSON
    ├── Models/
    │   ├── Mision.cs
    │   └── MisionRequest.cs                # cuerpo de POST/PUT
    ├── Data/misiones.json                  # almacenamiento
    ├── Program.cs                          # inyección de dependencias
    └── QuestLog.http                       # pruebas listas para usar
```

## Cómo ejecutar

Requiere el [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
cd QuestLog
dotnet run
```

La API queda en `http://localhost:5097`. Para probarla abre `QuestLog/QuestLog.http`
(Visual Studio 2022, o VS Code con la extensión *REST Client*), o usa `curl`/Postman.

## Endpoints

| Método | Ruta                | Función                    | Códigos                |
|--------|---------------------|----------------------------|------------------------|
| GET    | `/api/misiones`     | Obtener todas las misiones | 200                    |
| GET    | `/api/misiones/{id}`| Consultar una misión       | 200, 404               |
| POST   | `/api/misiones`     | Registrar una misión       | 201, 400               |
| PUT    | `/api/misiones/{id}`| Actualizar una misión      | 200, 400, 404          |
| DELETE | `/api/misiones/{id}`| Eliminar una misión        | 204, 404               |

Ejemplo de cuerpo para POST/PUT:

```json
{ "nombre": "El Despertar del Héroe", "dificultad": "Fácil", "recompensaXP": 100 }
```

## Reglas de negocio (en `MisionService`)

- El nombre es obligatorio.
- La dificultad solo puede ser `Fácil`, `Media` o `Difícil` (se acepta sin importar mayúsculas o acentos: `facil` → `Fácil`).
- La recompensa XP no puede ser negativa.
- Toda misión nueva inicia con `completada = false` (se ignora lo que envíe el cliente).
- No se puede modificar ni eliminar una misión inexistente (404).

Los errores se devuelven en formato `ProblemDetails` con un mensaje claro en `detail`.

## Notas de diseño

- `IMisionRepository` no menciona JSON, así que en la Etapa 2 solo se agrega otra implementación
  y se cambia **una línea** en `Program.cs`; controlador y servicio no se tocan.
- `MisionJsonRepository` es *Singleton* y serializa los accesos con un `SemaphoreSlim`;
  escribe a un archivo temporal y lo renombra para no corromper `misiones.json` si la app se cae.
- La ruta del archivo se puede cambiar con la clave `Persistencia:RutaJson` en `appsettings.json`.
- `Data/misiones.json` incluye 3 misiones de ejemplo (evidencia de la verificación de la etapa).
  Bórralo o vacíalo (`[]`) si quieres empezar desde cero.
