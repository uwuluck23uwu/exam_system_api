using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Domain.Interfaces
{
    public interface IUserSelectService
    {
        //// ดึงข้อสอบที่ทุกคนต้องสอบ พร้อมคำถาม (ฝั่งเจ้าหน้าที่ เผื่อต้องการดูข้อมูล) ***test***
        //Task<ResponseData> GetExampartWithQuestionAsync();

        // เลือกข้อสอบว่าจะทำข้อสอบชุดไหน กี่วัน วันที่เท่าไหร่บ้าง เพื่อที่จะนำไปคำนวนสร้างตาราง GenerateExamPart (ตารางไว้เก็บว่าทำข้อสอบวันที่เท่าไหร่บ้าง)
        Task<ResponseMessage> CreateAsync(EsExamUserSelection req, List<DateOnly> scheduledDate);

        // ดูชุดข้อสอบของตัวเองที่สอบไปแล้ว
        Task<List<dynamic>> GetResultExamSetByUserAsync(int userId);

        //// ดูชุดข้อสอบที่ตัวเองยังไม่ได้สอบ
        //Task<List<EsGeneratedExamPart>> GetExamWaitingByUserAsync(int userId);
    }
}
