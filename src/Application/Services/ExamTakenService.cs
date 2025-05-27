using Microsoft.EntityFrameworkCore;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Infrastructure.Persistence.Repositories
{
    public class ExamTakenService : IExamTakenService
    {
        private readonly IServiceFactory _service;

        public ExamTakenService(IServiceFactory service)
        {
            _service = service;
        }


    }
}
