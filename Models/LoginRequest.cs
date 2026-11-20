using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR01.Models;

public class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
