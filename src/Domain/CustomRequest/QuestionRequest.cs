using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Domain.CustomRequest
{
    public class QuestionRequest
    {
        public string QuestionText { get; set; } = null!;

        public string QuestionType { get; set; } = null!;

        public string? StdGroup { get; set; }

        public string? StdType { get; set; }

        public string? RefStdId { get; set; }

        public string Difficulty { get; set; } = null!;

        public string Subject { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public string? ApprovedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public List<EsChoiceRequest> Choices { get; set; } = new List<EsChoiceRequest>();
    }

    public class EsChoiceRequest
    {
        public int ChoiceId { get; set; }

        public int QuestionId { get; set; }

        public string ChoiceText { get; set; } = null!;

        public bool IsCorrect { get; set; }
    }
}
