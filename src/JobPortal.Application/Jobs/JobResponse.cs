using JobPortal.Application.Categories;
using JobPortal.Domain.Enums;

namespace JobPortal.Application.Jobs;

public class JobResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Requirements { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public CategoryResponse Category { get; set; } = null!;

    public EmploymentType EmploymentType { get; set; }

    public ExperienceLevel ExperienceLevel { get; set; }

    public decimal? Salary { get; set; }

    public DateTime Deadline { get; set; }

    public JobStatus Status { get; set; }

    public Guid EmployerId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
