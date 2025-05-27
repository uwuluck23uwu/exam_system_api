namespace EXAM_SYSTEM_API.Domain.CustomRequest
{
    public class ExamScheduleResponse
    {
        /// <summary>
        /// รหัสการจัดตารางสอบ
        /// </summary>
        public int ScheduleId { get; set; }


        /// <summary>
        /// ครั้งที่สอบ เช่น ครั้งที่ 1, 2, 3 เป็นต้น
        /// </summary>
        public int PartNo { get; set; }

        /// <summary>
        /// วันที่สอบ
        /// </summary>
        public string ScheduleDate { get; set; }

        /// <summary>
        /// เวลาเริ่มสอบ
        /// </summary>
        public TimeOnly StartTime { get; set; }

        /// <summary>
        /// เวลาสิ้นสุดการสอบ
        /// </summary>
        public TimeOnly EndTime { get; set; }

        /// <summary>
        /// จำนวนข้อ
        /// </summary>
        public int TotalQuestions { get; set; }

        /// <summary>
        /// สถานะ 0 คือ ยังไม่ได้สอบ , 1 คือ สอบแล้ว, 2 คือ ไม่ได้สอบ (ยกเลิการสอบอัตโนมัติ)
        /// </summary>
        public string Status { get; set; }
        public string StatusName { get; set; }

        public List<ExamRescheduleResponse> Reschedules { get; set; }
    }

    public class ExamRescheduleResponse
    {
        public string ScheduleDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Reason { get; set; }
    }
}
