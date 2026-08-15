using System.Text.Json.Serialization;
using JobPortal.Domain.Enums;

namespace JobPortal.Application.Auth;

public class UserResponse
{
    public Guid Id { get; set; }

    [JsonPropertyName("fullname")]
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("account_type")]
    public AccountType AccountType { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
