using JobPortal.Domain.Enums;

namespace JobPortal.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public AccountType AccountType { get; set; }

    public UserStatus Status { get; set; } = UserStatus.Active;

    public DateTime CreatedAtUtc { get; set; }
}
