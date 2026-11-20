using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR01.Models;

public class Evento
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [StringLength(50)]
    public string Categoria { get; set; } = string.Empty;

    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "El lugar es obligatorio.")]
    [StringLength(100)]
    public string Lugar { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser mayor que cero.")]
    public int CupoMaximo { get; set; }

    [Required(ErrorMessage = "La dificultad es obligatoria.")]
    [StringLength(20)]
    public string Dificultad { get; set; } = "Media";

    [Range(0, int.MaxValue, ErrorMessage = "La recompensa XP no puede ser negativa.")]
    public int RecompensaXP { get; set; }

    public bool Activo { get; set; } = true;
}
