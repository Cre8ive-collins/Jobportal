using JobPortal.Domain.Enums;

namespace JobPortal.Application.Users;

public class CreateUserRequest
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Student;
}