namespace EXAM_SYSTEM_API.Domain.CustomRequest
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
