namespace JobPortal.Domain.Entities;

public class JobSeekerProfile
{
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public string Skills { get; set; } = string.Empty;

    public string Education { get; set; } = string.Empty;

    public string Experience { get; set; } = string.Empty;

    public string Headline { get; set; } = string.Empty;

    public string? CvUrl { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
