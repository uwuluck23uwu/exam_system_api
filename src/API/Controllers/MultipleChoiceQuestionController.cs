using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EXAM_SYSTEM_API.API.Shared;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.API.Controllers
{
    /// <summary>
    /// controller
    /// </summary>
    [ApiExplorerSettings(GroupName = "MultipleChoiceQuestion-คำถาม(ใหม่)")]
    [Route("api/[controller]")]
    [ApiController]
    public class MultipleChoiceQuestionController : ControllerBase
    {

        private readonly IGenericCrudService<EsMultipleChoiceQuestion> _service;
        private readonly IMultipleChoiceQuestionService _askService;
        private readonly ControllerHelper _helper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="service"></param>
        /// <param name="askService"></param>
        /// <param name="helper"></param>
        public MultipleChoiceQuestionController(
            IGenericCrudService<EsMultipleChoiceQuestion> service,
            IMultipleChoiceQuestionService askService,
            ControllerHelper helper
        )
        {
            _service = service;
            _askService = askService;
            _helper = helper;
        }

        /// <summary>
        /// ดึงข้อมูลคำถาม (ใหม่)
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMultipleChoiceQuestion([Required] int pageSize = 10, [Required] int currentPage = 1)
        {
            return await _helper.HandleRequest(() => _service.GetAllPageAsync(pageSize, currentPage));
        }

        /// <summary>
        /// ดึงข้อมูลคำถาม (ใหม่) แบบตาม id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            return Ok(new ResponseData(200, true, "Success", item));
        }

        /// <summary>
        /// สร้างข้อมูลคำถาม
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] List<EsMultipleChoiceQuestion> req)
        {
            return await _helper.HandleRequest(() => _service.AddRangeAsync(req));
        }

        /// <summary>
        /// แก้ไขข้อมูลคำถาม
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] EsMultipleChoiceQuestion req)
        {
            if (id != req.QuestionId) return BadRequest("Mismatched ID");
            return await _helper.HandleRequest(() => _service.UpdateAsync(req));
        }

        /// <summary>
        /// ลบข้อมูลคำถาม
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _helper.HandleRequest(() => _service.DeleteAsync(id));
        }

        /// <summary>
        /// ข้อมูลคำถาม
        /// </summary>
        /// <param name="enrollmentId"></param>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        [HttpGet("GetRandomAsk")]
        public async Task<IActionResult> GetRandomAsk(int enrollmentId, int scheduleId = 0)
        {
            return await _helper.HandleRequest(() => _askService.GetRandomAsk(enrollmentId, scheduleId));
        }

        [HttpPost("SaveExamQuiz")]
        public async Task<IActionResult> SaveExamQuiz([FromBody] ExamQuizReq req)
        {
            return await _helper.HandleRequest(() => _askService.SaveExamQuiz(req));
        }

        [HttpGet("ReportUocExamQuiz")]
        public async Task<IActionResult> ReportUocExamQuiz(int enrollmentId = 0, int scheduleId = 0)
        {
            return await _helper.HandleRequest(() => _askService.ReportUocExamQuiz(enrollmentId, scheduleId));
        }

        [HttpGet("GetAllExamHistory")]
        public async Task<IActionResult> GetAllExamHistory([FromQuery] ExamHistoryReq req)
        {
            return await _helper.HandleRequest(() => _askService.GetAllExamHistory(req));
        }

    }
}
