using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Application.Interfaces
{
    public interface IExamInstructionService
    {
        Task<ResponseData> GetExamInstructionByExamsetIdAsync(int examsetId);
    }
}
