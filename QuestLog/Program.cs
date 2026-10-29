using QuestLog.Repositories;
using QuestLog.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---- Inyección de dependencias -------------------------------------------
// ETAPA 1: persistencia en archivo JSON.
// Singleton: el repositorio protege el archivo con un SemaphoreSlim único.
builder.Services.AddSingleton<IMisionRepository, MisionJsonRepository>();
// ETAPA 2: aquí solo cambiará esta línea por el repositorio de EF Core.
builder.Services.AddScoped<IMisionService, MisionService>();
// ---------------------------------------------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");
app.MapControllers();

// Respuesta genérica (500) para errores no previstos, sin filtrar detalles internos.
app.Map("/error", () => Results.Problem(
    title: "Error interno del servidor",
    statusCode: StatusCodes.Status500InternalServerError));

app.Run();
