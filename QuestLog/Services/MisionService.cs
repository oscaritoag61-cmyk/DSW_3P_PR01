using System.Globalization;
using System.Text;
using QuestLog.Models;
using QuestLog.Repositories;

namespace QuestLog.Services;

public class MisionService : IMisionService
{
    private static readonly string[] DificultadesValidas = { "Fácil", "Media", "Difícil" };

    private readonly IMisionRepository _repositorio;

    public MisionService(IMisionRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public Task<IReadOnlyList<Mision>> ObtenerTodasAsync() => _repositorio.ObtenerTodasAsync();

    public async Task<Mision> ObtenerPorIdAsync(int id)
    {
        return await _repositorio.ObtenerPorIdAsync(id)
               ?? throw new MisionNoEncontradaException(id);
    }

    public async Task<Mision> CrearAsync(MisionRequest datos)
    {
        var mision = ConstruirMisionValidada(datos);

        // Regla: toda misión nueva inicia como no completada,
        // sin importar lo que envíe el cliente.
        mision.Completada = false;

        return await _repositorio.AgregarAsync(mision);
    }

    public async Task<Mision> ActualizarAsync(int id, MisionRequest datos)
    {
        if (datos.Id is not null && datos.Id != 0 && datos.Id != id)
            throw new ValidacionException("El id del cuerpo no coincide con el id de la ruta.");

        // Regla: no se modifican misiones inexistentes.
        var existente = await _repositorio.ObtenerPorIdAsync(id)
                        ?? throw new MisionNoEncontradaException(id);

        var mision = ConstruirMisionValidada(datos);
        mision.Id = id;
        // Si no se envía "completada", se conserva el valor actual.
        mision.Completada = datos.Completada ?? existente.Completada;

        if (!await _repositorio.ActualizarAsync(mision))
            throw new MisionNoEncontradaException(id);

        return mision;
    }

    public async Task EliminarAsync(int id)
    {
        // Regla: no se eliminan misiones inexistentes.
        if (!await _repositorio.EliminarAsync(id))
            throw new MisionNoEncontradaException(id);
    }

    // ---- Validación de reglas de negocio ----

    private static Mision ConstruirMisionValidada(MisionRequest datos)
    {
        if (string.IsNullOrWhiteSpace(datos.Nombre))
            throw new ValidacionException("El nombre de la misión es obligatorio.");

        if (string.IsNullOrWhiteSpace(datos.Dificultad))
            throw new ValidacionException("La dificultad es obligatoria. Valores permitidos: Fácil, Media o Difícil.");

        var dificultad = NormalizarDificultad(datos.Dificultad)
            ?? throw new ValidacionException("Dificultad inválida. Valores permitidos: Fácil, Media o Difícil.");

        if (datos.RecompensaXP is null)
            throw new ValidacionException("La recompensa de XP es obligatoria.");

        if (datos.RecompensaXP < 0)
            throw new ValidacionException("La recompensa de XP no puede ser negativa.");

        return new Mision
        {
            Nombre = datos.Nombre.Trim(),
            Dificultad = dificultad,
            RecompensaXP = datos.RecompensaXP.Value
        };
    }

    /// <summary>
    /// Acepta "facil", "FÁCIL", "dificil"... y devuelve el valor canónico
    /// ("Fácil", "Media", "Difícil"), o null si no es válido.
    /// </summary>
    private static string? NormalizarDificultad(string valor)
    {
        var buscado = QuitarAcentos(valor.Trim());
        return DificultadesValidas.FirstOrDefault(d =>
            string.Equals(QuitarAcentos(d), buscado, StringComparison.OrdinalIgnoreCase));
    }

    private static string QuitarAcentos(string texto)
    {
        var descompuesto = texto.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(descompuesto.Length);
        foreach (var c in descompuesto)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
