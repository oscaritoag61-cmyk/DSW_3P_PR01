namespace QuestLog.Models;

/// <summary>
/// Misión disponible dentro del videojuego.
/// </summary>
public class Mision
{
    /// <summary>Identificador único (lo asigna el repositorio).</summary>
    public int Id { get; set; }

    /// <summary>Nombre de la misión (obligatorio).</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Fácil, Media o Difícil.</summary>
    public string Dificultad { get; set; } = string.Empty;

    /// <summary>Experiencia (XP) otorgada. No puede ser negativa.</summary>
    public int RecompensaXP { get; set; }

    /// <summary>Indica si la misión fue completada.</summary>
    public bool Completada { get; set; }
}
