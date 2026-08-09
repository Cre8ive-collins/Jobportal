using System.Text.Json.Serialization;

namespace JobPortal.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<UserRole>))]
public enum UserRole
{
    Admin,
    Student
}