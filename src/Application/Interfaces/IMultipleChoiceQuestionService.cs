using EXAM_SYSTEM_API.Application.Shared;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.CustomResponse;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Application.Interfaces
{
    public interface IMultipleChoiceQuestionService
    {
        Task<ResponseData> GetRandomAsk(int enrollmentId, int scheduleId);
        Task<ResponseMessage> SaveExamQuiz(ExamQuizReq req);
        Task<ResponseData> ReportUocExamQuiz(int? enrollmentId, int? scheduleId);
        Task<PaginationResponse<VEsExamScheduleDetail>> GetAllExamHistory(ExamHistoryReq req);

    }
}
