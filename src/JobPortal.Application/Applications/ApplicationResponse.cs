using JobPortal.Domain.Enums;

namespace JobPortal.Application.Applications;

public class ApplicationResponse
{
    public Guid JobId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public Guid ApplicantId { get; set; }

    public string ApplicantName { get; set; } = string.Empty;

    public string ApplicantEmail { get; set; } = string.Empty;

    public ApplicationStatus Status { get; set; }

    public DateTime AppliedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
