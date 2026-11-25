using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR01.Models;

public class Mision
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dificultad es obligatoria.")]
    [StringLength(20)]
    public string Dificultad { get; set; } = "Media";

    [Range(0, int.MaxValue, ErrorMessage = "La recompensa XP no puede ser negativa.")]
    public int RecompensaXP { get; set; }

    public bool Completada { get; set; }
}
