using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using JobPortal.Domain.Enums;

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
    [EnumDataType(typeof(AccountType))]
    [JsonPropertyName("account_type")]
    public AccountType? AccountType { get; set; }
}
