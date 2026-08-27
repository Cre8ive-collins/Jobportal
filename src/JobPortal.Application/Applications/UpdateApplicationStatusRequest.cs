using System.ComponentModel.DataAnnotations;
using JobPortal.Domain.Enums;

namespace JobPortal.Application.Applications;

public class UpdateApplicationStatusRequest
{
    [Required]
    [EnumDataType(typeof(ApplicationStatus))]
    public ApplicationStatus? Status { get; set; }
}
