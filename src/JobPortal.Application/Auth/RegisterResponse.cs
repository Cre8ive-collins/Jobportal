namespace JobPortal.Application.Auth;

public class RegisterResponse
{
    public string Message { get; set; } = "Registration successful.";

    public required UserResponse User { get; set; }
}
