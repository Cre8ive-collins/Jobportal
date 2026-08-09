using JobPortal.Domain.Enums;
namespace JobPortal.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Student;

    public UserStatus Status { get; set; } = UserStatus.MustChangePassword;

    public DateTime CreatedAtUtc { get; set; }
}
