using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EXAM_SYSTEM_API.API.Shared;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Services;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.API.Controllers
{
    /// <summary>
    /// controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GcioCandidateController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly ControllerHelper _helper;
        private readonly IGcioCandidateService _candidateService;

        /// <summary>
        /// constructor
        /// </summary>
        public GcioCandidateController(IServiceFactory serviceFactory, ControllerHelper helper, IGcioCandidateService candidateService)
        {
            _serviceFactory = serviceFactory;
            _helper = helper;
            _candidateService = candidateService;
        }

        /// <summary>
        /// ดึงข้อมูล candidate แบบ iseUsed เป็น true
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCandidate()
        {
            return await _helper.HandleRequest(() =>
            _candidateService.GetCandidateAsync());
        }

        /// <summary>
        /// ดึงข้อมูล candidate ตาม email กับ mobilePhone
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCandidateById([Required] string email, [Required] string mobilePhone)
        {
            return await _helper.HandleRequest(() =>
            _candidateService.GetCandidateByIdAsync(email, mobilePhone));
        }

        /// <summary>
        /// เพิ่มข้อมูล candidate
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCandidate([FromForm] EsGcioCandidate req)
        {
            return Ok(await _serviceFactory.GetService<EsGcioCandidate>()
                .AddAsync(req));
        }

        /// <summary>
        /// แก้ไขข้อมูล candidate
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCandidate([FromForm] EsGcioCandidate req)
        {
            return Ok(await _serviceFactory.GetService<EsGcioCandidate>()
                .UpdateAsync(req));
        }

        /// <summary>
        /// ลบข้อมูล candidate (ไม่ได้ลบจริง แค่เปลี่ยนสถานะเป็น false)
        /// </summary>
        /// <param name="candidateId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteCandidate(int candidateId)
        {
            return await _helper.HandleRequest(() =>
            _candidateService.DeleteCandidateAsync(candidateId));
        }

    }
}
