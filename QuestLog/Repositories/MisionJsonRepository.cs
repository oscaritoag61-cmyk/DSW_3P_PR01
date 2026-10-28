using System.Text.Encodings.Web;
using System.Text.Json;
using QuestLog.Models;

namespace QuestLog.Repositories;

/// <summary>
/// Persistencia en un archivo misiones.json. Se registra como Singleton
/// porque el SemaphoreSlim debe ser único para serializar los accesos al archivo.
/// </summary>
public class MisionJsonRepository : IMisionRepository
{
    private static readonly JsonSerializerOptions OpcionesJson = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        // Evita que "Fácil" se guarde como "Fácil".
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly string _rutaArchivo;
    private readonly SemaphoreSlim _candado = new(1, 1);

    public MisionJsonRepository(IConfiguration configuracion, IHostEnvironment entorno)
    {
        var ruta = configuracion["Persistencia:RutaJson"] ?? Path.Combine("Data", "misiones.json");
        _rutaArchivo = Path.IsPathRooted(ruta) ? ruta : Path.Combine(entorno.ContentRootPath, ruta);

        var carpeta = Path.GetDirectoryName(_rutaArchivo);
        if (!string.IsNullOrEmpty(carpeta))
            Directory.CreateDirectory(carpeta);

        if (!File.Exists(_rutaArchivo))
            File.WriteAllText(_rutaArchivo, "[]");
    }

    public async Task<IReadOnlyList<Mision>> ObtenerTodasAsync()
    {
        await _candado.WaitAsync();
        try
        {
            var misiones = await LeerAsync();
            return misiones.OrderBy(m => m.Id).ToList();
        }
        finally { _candado.Release(); }
    }

    public async Task<Mision?> ObtenerPorIdAsync(int id)
    {
        await _candado.WaitAsync();
        try
        {
            var misiones = await LeerAsync();
            return misiones.FirstOrDefault(m => m.Id == id);
        }
        finally { _candado.Release(); }
    }

    public async Task<Mision> AgregarAsync(Mision mision)
    {
        await _candado.WaitAsync();
        try
        {
            var misiones = await LeerAsync();
            mision.Id = misiones.Count == 0 ? 1 : misiones.Max(m => m.Id) + 1;
            misiones.Add(mision);
            await GuardarAsync(misiones);
            return mision;
        }
        finally { _candado.Release(); }
    }

    public async Task<bool> ActualizarAsync(Mision mision)
    {
        await _candado.WaitAsync();
        try
        {
            var misiones = await LeerAsync();
            var indice = misiones.FindIndex(m => m.Id == mision.Id);
            if (indice < 0) return false;

            misiones[indice] = mision;
            await GuardarAsync(misiones);
            return true;
        }
        finally { _candado.Release(); }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        await _candado.WaitAsync();
        try
        {
            var misiones = await LeerAsync();
            var eliminadas = misiones.RemoveAll(m => m.Id == id);
            if (eliminadas == 0) return false;

            await GuardarAsync(misiones);
            return true;
        }
        finally { _candado.Release(); }
    }

    // ---- Acceso al archivo (siempre se llama con el candado tomado) ----

    private async Task<List<Mision>> LeerAsync()
    {
        if (!File.Exists(_rutaArchivo)) return new List<Mision>();

        var contenido = await File.ReadAllTextAsync(_rutaArchivo);
        if (string.IsNullOrWhiteSpace(contenido)) return new List<Mision>();

        return JsonSerializer.Deserialize<List<Mision>>(contenido, OpcionesJson) ?? new List<Mision>();
    }

    private async Task GuardarAsync(List<Mision> misiones)
    {
        // Escribe a un temporal y lo mueve encima: si la app se cae a la mitad
        // de la escritura, misiones.json no queda corrupto.
        var temporal = _rutaArchivo + ".tmp";
        var json = JsonSerializer.Serialize(misiones, OpcionesJson);
        await File.WriteAllTextAsync(temporal, json);
        File.Move(temporal, _rutaArchivo, overwrite: true);
    }
}
