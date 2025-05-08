using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXAM_SYSTEM_API.Application.Shared.Responses;

namespace EXAM_SYSTEM_API.Application.Interfaces
{
    public interface IGcioCandidateService
    {
        Task<ResponseData> GetCandidateAsync();
        Task<ResponseData> GetCandidateByIdAsync(string email, string mobilePhone);
        Task<ResponseMessage> DeleteCandidateAsync(int candidateId);
    }
}
