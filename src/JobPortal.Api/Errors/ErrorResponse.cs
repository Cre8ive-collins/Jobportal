namespace JobPortal.Api.Errors;

public class ErrorResponse
{
    public required string Message { get; set; }

    public IDictionary<string, string[]>? Errors { get; set; }
}
