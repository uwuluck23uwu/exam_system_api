using System;
using System.Collections.Generic;

namespace EXAM_SYSTEM_API.Domain.Entities;

public partial class VEsMultipleChoiceQuestionsEoc
{
    public int QuestionId { get; set; }

    public string QuestionText { get; set; } = null!;

    public string ChoiceA { get; set; } = null!;

    public string ChoiceB { get; set; } = null!;

    public string ChoiceC { get; set; } = null!;

    public string ChoiceD { get; set; } = null!;

    public string CorrectAnswer { get; set; } = null!;

    public string StdGroup { get; set; } = null!;

    public string StdType { get; set; } = null!;

    public string RefStdId { get; set; } = null!;

    public string? Difficulty { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public string IsUsed { get; set; } = null!;

    public string? EocStdCode { get; set; }

    public string? EocName { get; set; }

    public string? UocStdCode { get; set; }

    public string? UocName { get; set; }
}
