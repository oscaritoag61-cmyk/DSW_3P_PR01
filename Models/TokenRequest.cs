using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR01.Models;

public class TokenRequest
{
    [Required]
    public string ApiKey { get; set; } = string.Empty;
}
