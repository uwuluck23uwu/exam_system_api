using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Application.Services
{

    public class GcioCandidateService : IGcioCandidateService
    {
        private readonly IServiceFactory _service;

        public GcioCandidateService(IServiceFactory service)
        {
            _service = service;
        }

        public async Task<ResponseData> GetCandidateAsync()
        {
            var candidates = await _service.GetService<EsGcioCandidate>()
                .GetAll().Where(x => x.IsUsed).ToListAsync();

            return new ResponseData(200, true, "success", candidates);
        }

        public async Task<ResponseData> GetCandidateByIdAsync(string email, string mobilePhone)
        {
            try
            {
                var candidate = await _service.GetService<EsGcioCandidate>()
                .GetAll().FirstOrDefaultAsync(x => x.Email.Equals(email) && x.MobilePhone.Equals(mobilePhone) && x.IsUsed);

                if (candidate is null) return new ResponseData(200, false, "ไม่อนุญาตให้เข้าสอบ GCIO");

                return new ResponseData(200, true, "success", candidate);
            }
            catch (Exception ex)
            {
                return new ResponseData(400, false, "get is failed", ex);
            }
        }

        public async Task<ResponseMessage> DeleteCandidateAsync(int candidateId)
        {
            try
            {
                var candidate = await _service.GetService<EsGcioCandidate>()
                .GetByIdAsync(candidateId);

                if (candidate is null) return new ResponseMessage(200, false, "candidate not found");

                candidate.IsUsed = false;

                return await _service.GetService<EsGcioCandidate>().UpdateAsync(candidate);
            }
            catch (Exception ex)
            {
                return new ResponseMessage(400, false, "get is failed");
            }
        }

    }
}
