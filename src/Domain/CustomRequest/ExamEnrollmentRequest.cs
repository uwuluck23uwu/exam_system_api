using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Domain.CustomRequest
{
    public class ExamEnrollmentRequest
    {
        public EsExamEnrollment ExamEnrollment { get; set; }
        public List<ExamScheduleRequest> ExamSchedule { get; set; }
    }
}
