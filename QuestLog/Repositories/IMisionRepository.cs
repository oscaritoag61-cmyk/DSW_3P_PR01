using QuestLog.Models;

namespace QuestLog.Repositories;

/// <summary>
/// Contrato de persistencia de misiones. No sabe nada de JSON ni de SQL:
/// la Etapa 2 solo agrega otra implementación (EF Core) sin tocar
/// el servicio ni el controlador.
/// </summary>
public interface IMisionRepository
{
    Task<IReadOnlyList<Mision>> ObtenerTodasAsync();

    Task<Mision?> ObtenerPorIdAsync(int id);

    /// <summary>Guarda la misión, le asigna un Id nuevo y la devuelve.</summary>
    Task<Mision> AgregarAsync(Mision mision);

    /// <summary>Reemplaza los datos de la misión con ese Id. false si no existe.</summary>
    Task<bool> ActualizarAsync(Mision mision);

    /// <summary>Elimina la misión. false si no existe.</summary>
    Task<bool> EliminarAsync(int id);
}
