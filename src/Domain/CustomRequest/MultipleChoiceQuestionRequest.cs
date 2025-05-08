using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Domain.CustomRequest
{
    public class ExamQuizReq
    {
        public int EnrollmentId { get; set; }
        public int ScheduleId { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime StartedTime { get; set; }
        public DateTime SubmittedTime { get; set; }
        public List<ExamQuestionRequest> ListQuiz { get; set; } = new List<ExamQuestionRequest>();
    }

    public class ExamQuestionRequest
    {
        public int Id { get; set; }
        public string Answer { get; set; } = string.Empty;
    }

    public class ExamHistoryReq
    {
        public int PersonId { get; set; }
        public int PageSize { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
    }

}
