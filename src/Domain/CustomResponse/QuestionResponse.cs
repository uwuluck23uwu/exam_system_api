using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXAM_SYSTEM_API.Domain.CustomResponse
{
    public class QuestionResponse
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; } = null!;

        public string QuestionType { get; set; } = null!;

        public string? StdGroup { get; set; }

        public string? StdType { get; set; }

        public string? RefStdId { get; set; }

        public string Difficulty { get; set; } = null!;

        public string Subject { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }

        public List<ChoiceResponse> Choices { get; set; } = new List<ChoiceResponse>();
    }

    public class ChoiceResponse
    {
        public int ChoiceId { get; set; }

        public string ChoiceText { get; set; } = null!;
    }
}
