using Microsoft.EntityFrameworkCore;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Application.Services
{
    public class ExamSetService : IExamSetService
    {
        private readonly IServiceFactory _service;

        public ExamSetService(IServiceFactory service)
        {
            _service = service;
        }

        public async Task<ResponseData> GetByExamTypeIdAsync(int examsetType)
        {
            var result = await _service.GetService<EsExamSet>()
                .GetAll().FirstOrDefaultAsync(x => x.ExamsetType.Equals(examsetType) && x.IsUsed == "1");

            if (result is null) return new ResponseData(200, false, "examset not found");

            return new ResponseData(200, true, "success", result);
        }

        public async Task<ResponseMessage> IsUseExamsetAsync(int examsetId)
        {
            var examsets = await _service.GetService<EsExamSet>().GetAllAsync();

            var examset = examsets.FirstOrDefault(x => x.ExamsetId.Equals(examsetId));

            foreach (var item in examsets)
            {
                if (item.ExamsetType == examset.ExamsetType)
                {
                    if (item.ExamsetId == examsetId)
                    {
                        item.IsUsed = "1";
                    }
                    else
                    {
                        item.IsUsed = "0";
                    }
                }
            }

            return await _service.GetService<EsExamSet>().UpdateRangeAsync(examsets);
        }

    }
}
