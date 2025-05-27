using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using EXAM_SYSTEM_API.API.Shared;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Domain.CustomRequest;

namespace EXAM_SYSTEM_API.API.Controllers
{
    [ApiExplorerSettings(GroupName = "ExamSchedule-จัดตารางสอบ")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExamScheduleController : ControllerBase
    {
        private readonly IExamScheduleService _examScheduleService;
        private readonly ControllerHelper _helper;

        public ExamScheduleController(IExamScheduleService examScheduleService, ControllerHelper helper)
        {
            _examScheduleService = examScheduleService;
            _helper = helper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetMyExamEnrollment([Required] int personId,
            [Required] int examsetType, [Required] int pageSize = 10, [Required] int currentPage = 1)
        {
            return await _helper.HandleRequest(() =>
            _examScheduleService.GetMyExamEnrollmentAsync(personId, examsetType, pageSize, currentPage));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEnrollmentByScheduleId([Required] int scheduleId)
        {
            return await _helper.HandleRequest(() =>
            _examScheduleService.GetEnrollmentByScheduleIdAsync(scheduleId));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateMyExamEnrollment([FromBody] ExamEnrollmentRequest req)
        {
            return Ok(await _examScheduleService.CreateMyExamEnrollmentAsync(req));
        }

        [HttpPost("UpdateExamSchedule")]
        public async Task<IActionResult> UpdateExamSchedule([FromBody] ExamReschedulesRequest req)
        {
            return Ok(await _examScheduleService.UpdateExamSchedule(req));
        }
    }
}