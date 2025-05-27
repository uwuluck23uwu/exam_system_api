using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EXAM_SYSTEM_API.API.Shared;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.Entities;
using EXAM_SYSTEM_API.Domain.Interfaces;
using EXAM_SYSTEM_API.Infrastructure.Persistence.Repositories;

namespace EXAM_SYSTEM_API.API.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiExplorerSettings(GroupName = "ExamTaken-การสอบ")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExamTakenController : ControllerBase
    {
        private readonly IGenericCrudService<EsExamTaken> _service;
        private readonly ControllerHelper _helper;
        private readonly IExamTakenService _examTakenService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="service"></param>
        /// <param name="helper"></param>
        /// <param name="examTakenService"></param>
        public ExamTakenController(IGenericCrudService<EsExamTaken> service, ControllerHelper helper,
            IExamTakenService examTakenService)
        {
            _service = service;
            _helper = helper;
            _examTakenService = examTakenService;
        }

        ///// <summary>
        ///// ดึงข้อมูลชุดข้อสอบที่สอบไปแล้ว แบบ pagination
        ///// </summary>
        ///// <param name="pageSize"></param>
        ///// <param name="currentPage"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> GetAll([Required] int pageSize = 10, [Required] int currentPage = 1)
        //{
        //    return await _helper.HandleRequest(() => _service.GetAllPageAsync(pageSize, currentPage));
        //}

        /// <summary>
        /// ดึงข้อมูลชุดข้อสอบ แบบตาม id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            return Ok(new ResponseData(200, true, "Success", item));
        }

        /// <summary>
        /// สร้างข้อมูลชุดข้อสอบที่ได้ทำการสอบ
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        //[HttpPost("Create")]
        //public async Task<IActionResult> Create([FromForm] EsExamTaken req)
        //{
        //    //return await _helper.HandleRequest(() => _service.AddAsync(req));
        //    return Ok(await _examTakenService.Createasync(req));
        //}

        /// <summary>
        /// แก้ไขข้อมูลชุดข้อสอบ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] EsExamTaken req)
        {
            if (id != req.ExamTakenId) return BadRequest("Mismatched ID");
            return await _helper.HandleRequest(() => _service.UpdateAsync(req));
        }

        /// <summary>
        /// ลบข้อมูลชุดข้อสอบ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _helper.HandleRequest(() => _service.DeleteAsync(id));
        }
    }
}
