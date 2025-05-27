using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using EXAM_SYSTEM_API.API.Shared;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.Entities;
using EXAM_SYSTEM_API.Domain.Interfaces;

namespace EXAM_SYSTEM_API.API.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiExplorerSettings(GroupName = "ExamUserSelection-เกี่ยวกับผู้ใช้งาน")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExamUserSelectionController : ControllerBase
    {
        private readonly IGenericCrudService<EsExamUserSelection> _selectService;
        private readonly IGenericCrudService<EsGeneratedExamPart> _examPartService;
        private readonly IUserSelectService _userSelectService;
        private readonly ControllerHelper _helper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectService"></param>
        /// <param name="examPartService"></param>
        /// <param name="userSelectService"></param>
        /// <param name="helper"></param>
        public ExamUserSelectionController(IGenericCrudService<EsExamUserSelection> selectService,
            IGenericCrudService<EsGeneratedExamPart> examPartService,
            IUserSelectService userSelectService,
            ControllerHelper helper)
        {
            _selectService = selectService;
            _examPartService = examPartService;
            _userSelectService = userSelectService;
            _helper = helper;
        }

        ///// <summary>
        ///// ดึงข้อสอบที่ทุกคนต้องสอบ พร้อมคำถาม (ฝั่งเจ้าหน้าที่ เผื่อต้องการดูข้อมูล) ***test***
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetExampartWithQuestionAsync()
        //{
        //    return Ok(await _userSelectService.GetExampartWithQuestionAsync());
        //}

        /// <summary>
        /// ดึงข้อมูลชุดข้อสอบที่ได้เลือกเพื่อทำการสอบ แบบ pagination
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll([Required] int pageSize = 10, [Required] int currentPage = 1)
        {
            return await _helper.HandleRequest(() => _selectService.GetAllPageAsync(pageSize, currentPage));
        }

        /// <summary>
        /// ดึงข้อมูลชุดข้อสอบ ทั้งที่ได้ทำ และกำลังรอเพื่อทำ
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetGeneratedExamPartAll([Required] int pageSize = 10, [Required] int currentPage = 1)
        {
            return await _helper.HandleRequest(() => _examPartService.GetAllPageAsync(pageSize, currentPage));
        }

        /// <summary>
        /// เลือกข้อสอบว่าจะทำข้อสอบชุดไหน กี่วัน วันที่เท่าไหร่บ้าง เพื่อที่จะนำไปคำนวนสร้างตาราง GenerateExamPart (ตารางไว้เก็บว่าทำข้อสอบวันที่เท่าไหร่บ้าง)
        /// </summary>
        /// <param name="req"></param>
        /// <param name="scheduledDate"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseData>> Create([FromForm] EsExamUserSelection req,
            [FromForm] List<DateOnly> scheduledDate)
        {
            return Ok(await _userSelectService.CreateAsync(req, scheduledDate));
        }

        /// <summary>
        /// ดูชุดข้อสอบของตัวเองที่สอบไปแล้ว
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetResultExamSetByUser([Required] int userId)
        {
            return Ok(await _userSelectService.GetResultExamSetByUserAsync(userId));
        }

        ///// <summary>
        ///// ดูชุดข้อสอบที่ตัวเองยังไม่ได้สอบ
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetExamWaitingByUserAsync([Required] int userId)
        //{
        //    return Ok(await _userSelectService.GetExamWaitingByUserAsync(userId));
        //}
    }
}
