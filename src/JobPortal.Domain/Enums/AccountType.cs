using System.Text.Json.Serialization;

namespace JobPortal.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<AccountType>))]
public enum AccountType
{
    JobSeeker,
    Employer
}
