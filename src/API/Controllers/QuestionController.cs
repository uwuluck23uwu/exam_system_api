using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using EXAM_SYSTEM_API.API.Shared;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Services;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.Entities;
using EXAM_SYSTEM_API.Domain.Interfaces;

namespace EXAM_SYSTEM_API.API.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiExplorerSettings(GroupName = "Question-คำถาม")]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IGenericCrudService<EsQuestion> _questionServiceEnt;
        private readonly IGenericCrudService<EsChoice> _choiceService;
        private readonly IQuestionService _questionService;
        private readonly ControllerHelper _helper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="questionServiceEnt"></param>
        /// <param name="choiceService"></param>
        /// <param name="questionService"></param>
        /// <param name="helper"></param>
        public QuestionController(IGenericCrudService<EsQuestion> questionServiceEnt,
            IGenericCrudService<EsChoice> choiceService,
            IQuestionService questionService,
            ControllerHelper helper)
        {
            _questionServiceEnt = questionServiceEnt;
            _choiceService = choiceService;
            _questionService = questionService;
            _helper = helper;
        }

        /// <summary>
        /// กรองคำถาม แบบ paination ใช้ค้นหาเพื่อตรวจสอบว่ามีคำถามนี้ไปแล้วหรือยัง และมีตัวเลือกต่าง ๆที่ใช้ในการค้นหา
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="search"></param>
        /// <param name="questionType"></param>
        /// <param name="difficulty"></param>
        /// <param name="subject"></param>
        /// <returns>แสดงข้อมูลที่กรองตามเงื่อนไข</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> FilterQuestion([Required] int pageSize = 10, [Required] int currentPage = 1,
            string? search = null, string? questionType = null, string? difficulty = null, string? subject = null)
        {
            return Ok(await _questionService.FilterQuestionAsync(pageSize, currentPage,
                search, questionType, difficulty, subject));
        }

        /// <summary>
        /// ดึงข้อมูลคำถามทั้งหมด แบบ paination
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns>แสดงข้อมูลรูปแบบ pagination</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([Required] int pageSize = 10, [Required] int currentPage = 1)
        {
            return await _helper.HandleRequest(() => _questionServiceEnt.GetAllPageAsync(pageSize, currentPage));
        }

        ///// <summary>
        ///// สุ่มคำถามเพื่อที่จะสอบ ทำตอนกดเข้ามาสอบ (method นี้จะสุ่ม และสามารถนำข้อสอบที่ return ไปใช้งานได้เลย) **ข้อมูลที่ return ถ้าคำถามมา แล้วคำตอบไม่มา ให้ตรวจสอบว่าคำถามนั้นมีคำตอบหรือไม่**
        ///// </summary>
        ///// <param name="exam_part_id"></param>
        ///// <param name="num_question"></param>
        ///// <returns>แสดงข้อมูล คำถามพร้อมคำตอบ</returns>
        //[HttpPost("[action]")]
        //public async Task<IActionResult> RandomQuestions([Required] int exam_part_id,[Required] int num_question)
        //{
        //    return Ok(await _questionService.RandomQuestionsAsync(exam_part_id, num_question));
        //}

        /// <summary>
        /// สร้างคำถามพร้อมกับคำตอบของข้อสอบ กี่คำถาม และกี่คำตอบก็ได้
        /// </summary>
        /// <param name="req">ใส่เป็น Question แบบเป็น List พร้อม choice</param>
        /// <returns>200</returns>
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseData>> Create([FromBody] List<QuestionRequest> req)
        {
            return Ok(await _questionService.CreateAsync(req));
        }

        /// <summary>
        /// ลองหารจำนวนข้อสอบ (290 ข้อ หาร 3 เป็น 100 100 90 แต่ยังทำไม่ได้) ได้แค่ 97 97 96
        /// </summary>
        /// <param name="total"></param>
        /// <param name="parts"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult SplitNumber([Required] int total, [Required] int parts)
        {
            List<int> split = new List<int>();
            int baseValue = total / parts;
            int remainder = total % parts;

            for (int i = 0; i < parts; i++)
            {
                if (i < remainder)
                    split.Add(baseValue + 1); // เพิ่ม 1 ให้บางชุดกรณีหารไม่ลงตัว
                else
                    split.Add(baseValue);
            }

            return Ok(split);
        }
    }
}
