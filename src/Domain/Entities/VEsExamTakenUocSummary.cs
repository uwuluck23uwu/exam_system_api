using System;
using System.Collections.Generic;

namespace EXAM_SYSTEM_API.Domain.Entities;

public partial class VEsExamTakenUocSummary
{
    public int? PersonId { get; set; }

    public int? EnrollmentId { get; set; }

    public int ScheduleId { get; set; }

    public string? UocStdCode { get; set; }

    public string? UocName { get; set; }

    public int? QuestionTotal { get; set; }

    public int? Score { get; set; }

    public decimal? PercentageScore { get; set; }
}
