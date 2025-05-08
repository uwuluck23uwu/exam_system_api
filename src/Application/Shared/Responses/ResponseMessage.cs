namespace EXAM_SYSTEM_API.Application.Shared.Responses;

public class ResponseMessage : ResponseBase
{
    public ResponseMessage() { }

    public ResponseMessage(int statusCode, bool success, string message)
    {
        StatusCode = statusCode;
        Success = success;
        Message = message;
    }
}


// namespace EXAM_SYSTEM_API.Application.Shared.Responses;

// public class ResponseMessages : ResponseBase
// {
//     public class ResponseMessage : ResponseBase
//     {
//       public ResponseMessage(int statusCode, bool taskStatus, string message)
//           : base(statusCode, taskStatus, message) { }
//     }
// }
