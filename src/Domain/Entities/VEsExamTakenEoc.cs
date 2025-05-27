using System;
using System.Collections.Generic;

namespace EXAM_SYSTEM_API.Domain.Entities;

public partial class VEsExamTakenEoc
{
    public int? EnrollmentId { get; set; }

    public int ScheduleId { get; set; }

    public int? ExamTakenDetailId { get; set; }

    public int? ExamTakenId { get; set; }

    public int? QuestionId { get; set; }

    public string? QuestionText { get; set; }

    public string? Answer { get; set; }

    public int Score { get; set; }

    public string? StdGroup { get; set; }

    public string? StdType { get; set; }

    public string? RefStdId { get; set; }

    public string? EocStdCode { get; set; }

    public string? Difficulty { get; set; }

    public string? UocStdCode { get; set; }

    public string? UocName { get; set; }
}
