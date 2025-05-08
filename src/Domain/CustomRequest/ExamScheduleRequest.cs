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
}
