using System.ComponentModel.DataAnnotations;
using JobPortal.Domain.Enums;

namespace JobPortal.Application.Jobs;

public abstract class JobRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(10000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(10000)]
    public string Requirements { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string Location { get; set; } = string.Empty;

    [Required]
    public Guid? CategoryId { get; set; }

    [Required]
    [EnumDataType(typeof(EmploymentType))]
    public EmploymentType? EmploymentType { get; set; }

    [Required]
    [EnumDataType(typeof(ExperienceLevel))]
    public ExperienceLevel? ExperienceLevel { get; set; }

    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal? Salary { get; set; }

    [Required]
    public DateTime? Deadline { get; set; }

    [Required]
    [EnumDataType(typeof(JobStatus))]
    public JobStatus? Status { get; set; }
}
