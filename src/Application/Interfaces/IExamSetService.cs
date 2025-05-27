using EXAM_SYSTEM_API.Application.Shared.Responses;

namespace EXAM_SYSTEM_API.Application.Interfaces
{
    public interface IExamSetService
    {
        Task<ResponseData> GetByExamTypeIdAsync(int examsetType);

        Task<ResponseMessage> IsUseExamsetAsync(int examsetId);
    }
}
