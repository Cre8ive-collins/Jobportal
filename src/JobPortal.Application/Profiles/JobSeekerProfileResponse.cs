using System.Text.Json.Serialization;

namespace JobPortal.Application.Profiles;

public class JobSeekerProfileResponse
{
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }

    [JsonPropertyName("fullname")]
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Skills { get; set; } = string.Empty;

    public string Education { get; set; } = string.Empty;

    public string Experience { get; set; } = string.Empty;

    public string Headline { get; set; } = string.Empty;

    [JsonPropertyName("cv_url")]
    public string? CvUrl { get; set; }

    [JsonPropertyName("updated_at_utc")]
    public DateTime? UpdatedAtUtc { get; set; }
}
