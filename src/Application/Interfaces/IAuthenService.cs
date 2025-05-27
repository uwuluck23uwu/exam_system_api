using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.CustomRequest;

namespace EXAM_SYSTEM_API.Application.Interfaces
{
    public interface IAuthenService
    {
        Task<ResponseData> Login(LoginRequest loginRequestDTO);
        Task<ResponseData> Register(RegisterationRequest registerationRequestDTO);
    }
}
