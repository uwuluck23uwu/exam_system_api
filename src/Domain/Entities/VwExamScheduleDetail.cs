using System;
using System.Collections.Generic;

namespace EXAM_SYSTEM_API.Domain.Entities;

public partial class VwExamScheduleDetail
{
    public int ScheduleId { get; set; }

    public int EnrollmentId { get; set; }

    public int PartNo { get; set; }

    public DateOnly ScheduleDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int TotalQuestions { get; set; }

    public int Status { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? NumDays { get; set; }

    public int? PersonId { get; set; }

    public string? ExamsetName { get; set; }

    public string? StdName { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? MaxScore { get; set; }

    public int? NumDateExam { get; set; }

    public int? ExamsetType { get; set; }

    public decimal? PassPercentage { get; set; }

    public int? ExamTakenId { get; set; }

    public TimeOnly? StartedTime { get; set; }

    public TimeOnly? SubmittedTime { get; set; }

    public string? ExamTakenStatus { get; set; }

    public int? Score { get; set; }

    public int? TotalScore { get; set; }

    public int? PercentageScore { get; set; }
}
