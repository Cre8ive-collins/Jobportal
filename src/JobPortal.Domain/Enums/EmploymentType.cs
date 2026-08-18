using System.Text.Json.Serialization;

namespace JobPortal.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<EmploymentType>))]
public enum EmploymentType
{
    FullTime,
    PartTime,
    Contract,
    Internship
}
