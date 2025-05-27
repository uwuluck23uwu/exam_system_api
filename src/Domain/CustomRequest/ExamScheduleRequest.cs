using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXAM_SYSTEM_API.Domain.CustomRequest
{
    public class ExamScheduleRequest
    {
        public DateOnly ScheduleDate { get; set; } = new DateOnly();
        public TimeOnly ScheduleStartTime { get; set; } = new TimeOnly();
        public TimeOnly ScheduleEndTime { get; set; } = new TimeOnly();
    }

    public class ExamReschedulesRequest
    {
        public int ScheduleId { get; set; }

        public DateOnly ScheduleDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}