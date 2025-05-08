namespace EXAM_SYSTEM_API.Application.Shared.Responses;

public class ResponseErrorMessages : ResponseBase
{
    public List<string> Messages { get; set; } = new();

    public ResponseErrorMessages(int statusCode, bool success, string message)
    {
        StatusCode = statusCode;
        Success = success;
        Messages.Add(message);
    }

    public ResponseErrorMessages(int statusCode, bool success, List<string> messages)
    {
        StatusCode = statusCode;
        Success = success;
        Messages = messages;
    }
}
