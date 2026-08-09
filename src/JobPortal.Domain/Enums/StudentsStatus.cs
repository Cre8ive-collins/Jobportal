using System.Text.Json.Serialization;

namespace JobPortal.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<StudentStatus>))]
public enum StudentStatus
{
    Active,
    Inactive,
    Suspended,
    Graduated
}