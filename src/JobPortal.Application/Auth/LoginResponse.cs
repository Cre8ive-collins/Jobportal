using System.Text.Json.Serialization;

namespace JobPortal.Application.Auth;

public class LoginResponse
{
    public string Message { get; set; } = "Login successful.";

    public required UserResponse User { get; set; }

    [JsonPropertyName("jwt_token")]
    public required string Token { get; set; }
}
