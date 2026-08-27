using System.ComponentModel.DataAnnotations;

namespace JobPortal.Application.Profiles;

public class UpdateJobSeekerProfileRequest
{
    [MaxLength(2000)]
    public string Skills { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string Education { get; set; } = string.Empty;

    [MaxLength(10000)]
    public string Experience { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Headline { get; set; } = string.Empty;
}
