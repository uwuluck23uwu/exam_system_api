using EXAM_SYSTEM_API.Application.Shared;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.CustomResponse;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Application.Interfaces
{
    public interface IQuestionService
    {
        // กรองคำถาม แบบ paination ใช้ค้นหาเพื่อตรวจสอบว่ามีคำถามนี้ไปแล้วหรือยัง และมีตัวเลือกต่าง ๆที่ใช้ในการค้นหา
        Task<PaginationResponse<QuestionResponse>> FilterQuestionAsync(int pageSize, int currentPage,
            string? search, string? questionType, string? difficulty, string? subject);

        //// สุ่มคำถามเพื่อที่จะสอบ ทำตอนกดเข้ามาสอบ (method นี้จะสุ่ม และสามารถนำข้อสอบที่ return ไปใช้งานได้เลย)
        //Task<ResponseData> RandomQuestionsAsync(int exam_part_id, int num_question);

        // สร้างคำถามพร้อมกับคำตอบของข้อสอบ กี่คำถาม และกี่คำตอบก็ได้
        Task<ResponseMessage> CreateAsync(List<QuestionRequest> req);
    }
}
