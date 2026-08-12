using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobPortal.Application.Auth;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(128)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [JsonPropertyName("fullname")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [JsonPropertyName("account_type")]
    public string AccountType { get; set; } = string.Empty;
}
