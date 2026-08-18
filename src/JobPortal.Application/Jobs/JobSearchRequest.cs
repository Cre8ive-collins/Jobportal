using System.ComponentModel.DataAnnotations;
using JobPortal.Domain.Enums;

namespace JobPortal.Application.Jobs;

public class JobSearchRequest
{
    [Range(1, 1000000)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 20;

    [MaxLength(200)]
    public string? Search { get; set; }

    public Guid? CategoryId { get; set; }

    [EnumDataType(typeof(EmploymentType))]
    public EmploymentType? EmploymentType { get; set; }

    [EnumDataType(typeof(ExperienceLevel))]
    public ExperienceLevel? ExperienceLevel { get; set; }
}
