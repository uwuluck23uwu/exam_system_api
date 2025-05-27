namespace EXAM_SYSTEM_API.Domain.CustomResponse
{
    public class ExamSetResponse
    {
        /// <summary>
        /// รหัสชุดข้อสอบ
        /// </summary>
        public int ExamsetId { get; set; }

        /// <summary>
        /// ประเภทชุดข้อสอบ
        /// </summary>
        public int ExamsetType { get; set; }

        /// <summary>
        /// ชื่อชุดข้อสอบ
        /// </summary>
        public string ExamsetName { get; set; } = null!;

        /// <summary>
        /// รหัสมาตรฐาน
        /// </summary>
        public int StdId { get; set; }

        /// <summary>
        /// ชื่อมาตรฐาน
        /// </summary>
        public string StdName { get; set; } = null!;

        /// <summary>
        /// รายละเอียดของชุดข้อสอบ
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// จำนวนข้อสอบทั้งหมด
        /// </summary>
        public int TotalQuestions { get; set; }

        /// <summary>
        /// คะแนนเต็มของข้อสอบชุดนี้
        /// </summary>
        public int MaxScore { get; set; }

        /// <summary>
        /// เปอร์เซ็นต์ที่ต้องได้เพื่อผ่าน เช่น 60.00
        /// </summary>
        public decimal PassPercentage { get; set; }

        /// <summary>
        /// จำนวนวันที่ใช้สอบ
        /// </summary>
        public int NumDateExam { get; set; }

        public string IsUsed { get; set; } = null!;
    }
}
