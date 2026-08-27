using JobPortal.Domain.Enums;

namespace JobPortal.Domain.Entities;

public class JobApplication
{
    public Guid JobId { get; set; }

    public Job Job { get; set; } = null!;

    public Guid ApplicantId { get; set; }

    public User Applicant { get; set; } = null!;

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Open;

    public DateTime AppliedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
