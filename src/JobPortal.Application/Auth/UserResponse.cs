using System.Text.Json.Serialization;

namespace JobPortal.Application.Auth;

public class UserResponse
{
    public Guid Id { get; set; }

    [JsonPropertyName("fullname")]
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("account_type")]
    public string AccountType { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
}
