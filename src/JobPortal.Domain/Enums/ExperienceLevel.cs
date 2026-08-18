using System.Text.Json.Serialization;

namespace JobPortal.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<ExperienceLevel>))]
public enum ExperienceLevel
{
    Entry,
    Mid,
    Senior
}
