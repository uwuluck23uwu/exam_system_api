using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using EXAM_SYSTEM_API.API.Shared;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.CustomResponse;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.API.Controllers
{
    /// <summary>
    /// controller
    /// </summary>
    [ApiExplorerSettings(GroupName = "ExamSchedule-จัดตารางสอบ")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExamScheduleController : ControllerBase
    {
        private readonly IExamScheduleService _examScheduleService;
        private readonly ControllerHelper _helper;

        /// <summary>
        /// constructor
        /// </summary>
        public ExamScheduleController(IExamScheduleService examScheduleService, ControllerHelper helper)
        {
            _examScheduleService = examScheduleService;
            _helper = helper;
        }

        /// <summary>
        /// ดูตารางสอบที่ตนเองได้จัดเอาไว้
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="examsetType"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMyExamEnrollment([Required] int personId,
            [Required] int examsetType, [Required] int pageSize = 10, [Required] int currentPage = 1)
        {
            return await _helper.HandleRequest(() =>
            _examScheduleService.GetMyExamEnrollmentAsync(personId, examsetType, pageSize, currentPage));
        }

        /// <summary>
        /// ดึงข้อมูล enrollment โดย scheduleId
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetEnrollmentByScheduleId([Required] int scheduleId)
        {
            return await _helper.HandleRequest(() =>
            _examScheduleService.GetEnrollmentByScheduleIdAsync(scheduleId));
        }

        /// <summary>
        /// จัดตารางสอบของตนเอง
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateMyExamEnrollment([FromBody] ExamEnrollmentRequest req)
        {
            return Ok(await _examScheduleService.CreateMyExamEnrollmentAsync(req));
        }
    }
}
