using System.Text.Json.Serialization;

namespace JobPortal.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<ApplicationStatus>))]
public enum ApplicationStatus
{
    Open,
    Hired,
    Rejected,
    Reviewed,
    Interviewing,
    Withdrawn
}
