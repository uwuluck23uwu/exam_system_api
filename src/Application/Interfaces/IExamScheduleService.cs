using EXAM_SYSTEM_API.Application.Shared;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.CustomResponse;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Application.Interfaces
{
    public interface IExamScheduleService
    {
        // ดูตารางสอบที่ตนเองได้จัดเอาไว้
        Task<PaginationResponse<ExamEnrollmentGroupResponse>> GetMyExamEnrollmentAsync(
                int personId, int examSetType, int pageSize, int currentPage);

        // ดึงข้อมูล enrollment โดย scheduleId
        Task<ResponseData> GetEnrollmentByScheduleIdAsync(int scheduleId);

        // จัดตารางสอบของตนเอง
        Task<ResponseMessage> CreateMyExamEnrollmentAsync(ExamEnrollmentRequest req);

        //ขอเลื่อนการสอบ
        Task<ResponseMessage> UpdateExamSchedule(ExamReschedulesRequest req);
    }
}