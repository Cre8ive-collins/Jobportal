using JobPortal.Domain.Entities;

namespace JobPortal.Application.Users;

public class CreateUserResult
{
    public required User User { get; set; }

    public required string TemporaryPassword { get; set; }
}