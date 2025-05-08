using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Domain.CustomResponse
{
    public class ExamEnrollmentGroupResponse
    {
        public string CreatedDate { get; set; }
        public List<ExamEnrollmentWithExamScheduleResponse> ExamEnrollments { get; set; }


    }

    public class ExamEnrollmentWithExamScheduleResponse
    {
        public int EnrollmentId { get; set; }
        public int NumDays { get; set; }
        public ExamSetResponse ExamSet { get; set; }

        public List<ExamScheduleResponse> ExamSchedules { get; set; }
    }

    // ของพี่แจ็ก (ที่พี่แจ็กให้ทำ)
    //public class ExamEnrollmentWithExamScheduleResponse
    //{
    //    public DateTime CreatedDate { get; set; }
    //    public string ExamsetTypeName { get; set; }
    //    public List<ExamScheduleResponse> ExamSchedules { get; set; }
    //}
}
