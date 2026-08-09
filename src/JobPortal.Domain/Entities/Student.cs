using JobPortal.Domain.Enums;
namespace JobPortal.Domain.Entities;

public class Student
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public StudentStatus Status { get; set; } = StudentStatus.Inactive;

    public DateTime CreatedAtUtc { get; set; }
}