using JobPortal.Domain.Enums;

namespace JobPortal.Application.Users;

public class UserResponse
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public UserStatus Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}