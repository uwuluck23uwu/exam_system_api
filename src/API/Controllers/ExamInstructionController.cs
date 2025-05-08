using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using EXAM_SYSTEM_API.API.Shared;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.API.Controllers
{
    /// <summary>
    /// controller
    /// </summary>
    [ApiExplorerSettings(GroupName = "ExamInstruction-คำชี้แจงการสอบ")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExamInstructionController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly ControllerHelper _helper;
        private readonly IExamInstructionService _examInstructionService;

        /// <summary>
        /// constructor
        /// </summary>
        public ExamInstructionController(IServiceFactory serviceFactory, ControllerHelper helper,
            IExamInstructionService examInstructionService)
        {
            _serviceFactory = serviceFactory;
            _helper = helper;
            _examInstructionService = examInstructionService;
        }

        /// <summary>
        /// ดึงข้อมูลคำชี้แจง แบบ pagination
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll([Required] int pageSize = 10, [Required] int currentPage = 1)
        {
            return await _helper.HandleRequest(() =>
            _serviceFactory.GetService<EsExamInstruction>().GetAllPageAsync(pageSize, currentPage));
        }

        /// <summary>
        /// ดึงข้อมูลคำชี้แจงโดย id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData>> GetById(int id)
        {
            var item = await _serviceFactory.GetService<EsExamInstruction>().GetByIdAsync(id);
            if (item == null) return NotFound();

            return Ok(new ResponseData(200, true, "Success", item));
        }

        /// <summary>
        /// ดึงข้อมูลคำชี้แจงโดย id
        /// </summary>
        /// <param name="examsetId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetExamInstructionByExamsetId(int examsetId)
        {
            return await _helper.HandleRequest(() =>
            _examInstructionService.GetExamInstructionByExamsetIdAsync(examsetId));
        }

        /// <summary>
        /// สร้างข้อมูลคำชี้แจง
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm] EsExamInstruction req)
        {
            return await _helper.HandleRequest(() =>
            _serviceFactory.GetService<EsExamInstruction>().AddAsync(req));
        }

        /// <summary>
        /// แก้ไขข้อมูลคำชี้แจง
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] EsExamInstruction req)
        {
            if (id != req.ExamsetId) return BadRequest("Mismatched ID");
            return await _helper.HandleRequest(() =>
            _serviceFactory.GetService<EsExamInstruction>().UpdateAsync(req));
        }

        /// <summary>
        /// ลบข้อมูลชุดข้อสอบ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _helper.HandleRequest(() =>
            _serviceFactory.GetService<EsExamInstruction>().DeleteAsync(id));
        }

    }
}
