using QuestLog.Models;

namespace QuestLog.Services;

/// <summary>
/// Lógica de negocio de QuestLog. Lanza <see cref="ValidacionException"/>
/// o <see cref="MisionNoEncontradaException"/>; el controlador las traduce a HTTP.
/// </summary>
public interface IMisionService
{
    Task<IReadOnlyList<Mision>> ObtenerTodasAsync();

    /// <exception cref="MisionNoEncontradaException"/>
    Task<Mision> ObtenerPorIdAsync(int id);

    /// <exception cref="ValidacionException"/>
    Task<Mision> CrearAsync(MisionRequest datos);

    /// <exception cref="ValidacionException"/>
    /// <exception cref="MisionNoEncontradaException"/>
    Task<Mision> ActualizarAsync(int id, MisionRequest datos);

    /// <exception cref="MisionNoEncontradaException"/>
    Task EliminarAsync(int id);
}
