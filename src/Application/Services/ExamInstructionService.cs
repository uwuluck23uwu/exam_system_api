using Microsoft.EntityFrameworkCore;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Application.Services
{
    public class ExamInstructionService : IExamInstructionService
    {
        private readonly IServiceFactory _service;

        public ExamInstructionService(IServiceFactory service)
        {
            _service = service;
        }

        public async Task<ResponseData> GetExamInstructionByExamsetIdAsync(int examsetId)
        {
            var result = await _service.GetService<EsExamInstruction>()
                .GetAll().FirstOrDefaultAsync(x => x.ExamsetId.Equals(examsetId));

            if (result == null) return new ResponseData(200, false, "examInstruction not found");

            return new ResponseData(200, true, "success", result);
        }
    }
}
