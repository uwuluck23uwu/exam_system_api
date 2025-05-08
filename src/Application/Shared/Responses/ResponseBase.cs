using System.Text.Json.Serialization;

namespace EXAM_SYSTEM_API.Application.Shared.Responses
{
  public abstract class ResponseBase
  {
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public ResponseBase() { }

    public ResponseBase(int statusCode, bool success, string message)
    {
      StatusCode = statusCode;
      Success = success;
      Message = message;
    }
  }

  public class ResponseData : ResponseBase
  {
    public object Data { get; set; }

    public ResponseData(int statusCode, bool taskStatus, string message, object data = null!)
        : base(statusCode, taskStatus, message)
    {
      Data = data;
    }
  }

  public class ErrorModel
  {
    public string FieldName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
  }
}
