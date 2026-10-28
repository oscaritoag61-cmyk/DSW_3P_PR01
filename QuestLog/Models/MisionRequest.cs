namespace QuestLog.Models;

/// <summary>
/// Datos que el cliente envía en POST y PUT. Todo es nullable a propósito:
/// las reglas de negocio se validan en el Service (no en el binding),
/// así los errores salen con mensajes claros y un 400 consistente.
/// </summary>
public class MisionRequest
{
    /// <summary>Opcional. Si viene en un PUT debe coincidir con el id de la ruta.</summary>
    public int? Id { get; set; }

    public string? Nombre { get; set; }

    public string? Dificultad { get; set; }

    public int? RecompensaXP { get; set; }

    /// <summary>Se ignora en POST (toda misión nueva inicia como no completada).</summary>
    public bool? Completada { get; set; }
}
