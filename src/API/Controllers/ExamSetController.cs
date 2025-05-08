using Microsoft.AspNetCore.Mvc;
using EXAM_SYSTEM_API.Domain.Entities;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.API.Shared;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using System.ComponentModel.DataAnnotations;

namespace EXAM_SYSTEM_API.API.Controllers
{
    [ApiExplorerSettings(GroupName = "ExamSet")]
    [ApiController]
    [Route("api/[controller]")]
    public class ExamSetController : ControllerBase
    {
        private readonly IGenericCrudService<EsExamSet> _service;
        private readonly ControllerHelper _helper;
        private readonly IExamSetService _examSetService;

        public ExamSetController(IGenericCrudService<EsExamSet> service, ControllerHelper helper, IExamSetService examSetService)
        {
            _service = service;
            _helper = helper;
            _examSetService = examSetService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll([Required] int pageSize = 10, [Required] int currentPage = 1)
        {
            return await _helper.HandleRequest(() => _service.GetAllPageAsync(pageSize, currentPage));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByExamTypeId(int examsetType)
        {
            return await _helper.HandleRequest(() => _examSetService.GetByExamTypeIdAsync(examsetType));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> IsUseExamsetAsync(int examsetId)
        {
            return await _helper.HandleRequest(() => _examSetService.IsUseExamsetAsync(examsetId));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] EsExamSet req)
        {
            var result = await _service.AddAsync(req);

            if (req.IsUsed == "1")
            {
                await IsUseExamsetAsync(req.ExamsetId);
            }

            return await _helper.HandleRequest(() => Task.FromResult(result));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] EsExamSet req)
        {
            if (id != req.ExamsetId) return BadRequest("Mismatched ID");
            return await _helper.HandleRequest(() => _service.UpdateAsync(req));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _helper.HandleRequest(() => _service.DeleteAsync(id));
        }
    }
}
