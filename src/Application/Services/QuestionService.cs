using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Shared;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.CustomResponse;
using EXAM_SYSTEM_API.Domain.Entities;
using EXAM_SYSTEM_API.Domain.Interfaces;

namespace EXAM_SYSTEM_API.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IServiceFactory _service;

        public QuestionService(IServiceFactory service)
        {
            _service = service;
        }

        private IQueryable<QuestionResponse> GetQueryQuestion(int? num)
        {
            var iqueryable = _service.GetService<EsQuestion>().GetAll()
                                .Select(qt => new QuestionResponse
                                {
                                    QuestionId = qt.QuestionId,
                                    QuestionText = qt.QuestionText,
                                    QuestionType = qt.QuestionType,
                                    StdGroup = qt.StdGroup,
                                    StdType = qt.StdType,
                                    RefStdId = qt.RefStdId,
                                    Difficulty = qt.Difficulty,
                                    Subject = qt.Subject,
                                    CreatedDate = qt.CreatedDate,
                                    Choices = num == 1 ? _service.GetService<EsChoice>().GetAll()
                                    .Where(x => x.QuestionId == qt.QuestionId)
                                    .Select(x => new ChoiceResponse
                                    {
                                        ChoiceId = x.ChoiceId,
                                        ChoiceText = x.ChoiceText
                                    }).ToList() : new List<ChoiceResponse>()
                                });

            //(from qt in _context.EsQuestions
            // select new QuestionResponse
            // {
            //     QuestionId = qt.QuestionId,
            //     QuestionText = qt.QuestionText,
            //     QuestionType = qt.QuestionType,
            //     StdGroup = qt.StdGroup,
            //     StdType = qt.StdType,
            //     RefStdId = qt.RefStdId,
            //     Difficulty = qt.Difficulty,
            //     Subject = qt.Subject,
            //     CreatedDate = qt.CreatedDate,
            //     Choices = num == 1 ? _context.EsChoices
            //                     .Where(x => x.QuestionId == qt.QuestionId)
            //                     .Select(x => new ChoiceResponse
            //                     {
            //                         ChoiceId = x.ChoiceId,
            //                         ChoiceText = x.ChoiceText
            //                     })
            //                     .ToList() : new List<ChoiceResponse>()
            // });

            return iqueryable;
        }

        // กรองคำถาม แบบ paination ใช้ค้นหาเพื่อตรวจสอบว่ามีคำถามนี้ไปแล้วหรือยัง และมีตัวเลือกต่าง ๆที่ใช้ในการค้นหา
        public async Task<PaginationResponse<QuestionResponse>> FilterQuestionAsync(int pageSize, int currentPage,
            string? search, string? questionType, string? difficulty, string? subject)
        {
            var iqueryable = GetQueryQuestion(0);

            if (!string.IsNullOrEmpty(search))
                iqueryable = iqueryable.Where(_ => (_.QuestionText ?? "").Contains(search.Trim()));
            if (!string.IsNullOrEmpty(questionType))
                iqueryable = iqueryable.Where(x => x.QuestionType == questionType);
            if (!string.IsNullOrEmpty(difficulty))
                iqueryable = iqueryable.Where(x => x.Difficulty == difficulty);
            if (!string.IsNullOrEmpty(subject))
                iqueryable = iqueryable.Where(x => x.Subject == subject);

            var totalCount = await iqueryable.CountAsync();
            if (totalCount == 0)
            {
                new ResponseData(200, false, "Question not found", new List<object>());
            }

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var data = await iqueryable.Skip((currentPage - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

            return new PaginationResponse<QuestionResponse>(data, totalCount, currentPage, pageSize);
        }

        private async Task<List<QuestionResponse>> GetRandomQueryQuestion(int? num, int take)
        {
            var iqueryable = await _service.GetService<EsQuestion>().GetAll()
                                .OrderBy(x => Guid.NewGuid())
                                .Select(qt => new QuestionResponse
                                {
                                    QuestionId = qt.QuestionId,
                                    QuestionText = qt.QuestionText,
                                    QuestionType = qt.QuestionType,
                                    StdGroup = qt.StdGroup,
                                    StdType = qt.StdType,
                                    RefStdId = qt.RefStdId,
                                    Difficulty = qt.Difficulty,
                                    Subject = qt.Subject,
                                    CreatedDate = qt.CreatedDate,
                                    Choices = num == 1 ? _service.GetService<EsChoice>().GetAll()
                                                  .Where(x => x.QuestionId == qt.QuestionId)
                                                  .Select(x => new ChoiceResponse
                                                  {
                                                      ChoiceId = x.ChoiceId,
                                                      ChoiceText = x.ChoiceText
                                                  })
                                                  .ToList() : new List<ChoiceResponse>()
                                }).Take(take).ToListAsync();

            //await (from qt in _context.EsQuestions
            //              orderby Guid.NewGuid()
            //              select new QuestionResponse
            //              {
            //                  QuestionId = qt.QuestionId,
            //                  QuestionText = qt.QuestionText,
            //                  QuestionType = qt.QuestionType,
            //                  StdGroup = qt.StdGroup,
            //                  StdType = qt.StdType,
            //                  RefStdId = qt.RefStdId,
            //                  Difficulty = qt.Difficulty,
            //                  Subject = qt.Subject,
            //                  CreatedDate = qt.CreatedDate,
            //                  Choices = num == 1 ? _context.EsChoices
            //                                  .Where(x => x.QuestionId == qt.QuestionId)
            //                                  .Select(x => new ChoiceResponse
            //                                  {
            //                                      ChoiceId = x.ChoiceId,
            //                                      ChoiceText = x.ChoiceText
            //                                  })
            //                                  .ToList() : new List<ChoiceResponse>()
            //              }).Take(take).ToListAsync();

            return iqueryable;
        }

        //// สุ่มคำถามเพื่อที่จะสอบ ทำตอนกดเข้ามาสอบ (method นี้จะสุ่ม และสามารถนำข้อสอบที่ return ไปใช้งานได้เลย)
        //public async Task<ResponseData> RandomQuestionsAsync(int exam_part_id, int num_question)
        //{
        //    try
        //    {
        //        var ep = await _service.GetService<EsGeneratedExamPart>().GetByIdAsync(exam_part_id);

        //        var geq = _service.GetService<EsGeneratedExamQuestion>().GetAll();
        //        var countgeq = await geq.Where(x => x.GeneratedPartId == exam_part_id).ToListAsync();

        //        if(countgeq.Count > 0) return new ResponseData(200, true, "question is exist", countgeq);

        //        var rndQuestion = await GetRandomQueryQuestion(1, num_question);

        //        var examQuestion = rndQuestion
        //            .Select(item => new EsGeneratedExamQuestion
        //            {
        //                GeneratedPartId = exam_part_id,
        //                QuestionId = item.QuestionId,
        //            })
        //            .ToList();

        //        await _service.GetService<EsGeneratedExamQuestion>().AddRangeAsync(examQuestion);

        //        return new ResponseData(200, true, "Random question successfully", rndQuestion);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Create failed" + ex.Message);
        //    }
        //}

        // สร้างคำถามพร้อมกับคำตอบของข้อสอบ กี่คำถาม และกี่คำตอบก็ได้
        public async Task<ResponseMessage> CreateAsync(List<QuestionRequest> req)
        {
            try
            {
                foreach (var items in req)
                {
                    try
                    {
                        var qt = AddQuestion(items);

                        await _service.GetService<EsQuestion>().AddAsync(qt);
                        //if (result.StatusCode != 200) return BadRequest("Create failed");

                        var ch = AddChoices(items.Choices, qt.QuestionId);

                        await _service.GetService<EsChoice>().AddRangeAsync(ch);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Create failed" + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                return new ResponseMessage(500, false, ex.Message);
            }

            return new ResponseMessage(200, true, "Created Successfully");
        }

        private static EsQuestion AddQuestion(QuestionRequest question)
            => new EsQuestion
            {
                QuestionText = question.QuestionText,
                QuestionType = question.QuestionType,
                StdGroup = question.StdGroup,
                StdType = question.StdType,
                RefStdId = question.RefStdId,
                Difficulty = question.Difficulty,
                Subject = question.Subject,
                CreatedBy = question.CreatedBy,
                ApprovedBy = question.ApprovedBy,
                CreatedDate = question.CreatedDate,
                ApprovedAt = question.ApprovedAt,
            };

        private static List<EsChoice> AddChoices(List<EsChoiceRequest> choices, int questionId)
            => choices.Select(x => new EsChoice
            {
                ChoiceText = x.ChoiceText,
                IsCorrect = x.IsCorrect,
                QuestionId = questionId,
            }).ToList();
    }
}

//public async Task<ResponseMessage> CreateAsync(List<QuestionRequest> req)
//{
//    try
//    {
//        using (var transaction = await _context.Database.BeginTransactionAsync())
//        {
//            foreach (var items in req)
//            {
//                try
//                {
//                    var qt = EsQuestion.AddQuestion(items);

//                    await _questionService.AddAsync(qt);
//                    //if (result.StatusCode != 200) return BadRequest("Create failed");

//                    var ch = EsQuestion.AddChoices(items.Choices, qt.QuestionId);

//                    await _choiceService.AddRangeAsync(ch);
//                }
//                catch (Exception ex)
//                {
//                    await transaction.RollbackAsync();
//                    throw new Exception("Create failed" + ex.Message);
//                }
//            }

//            await transaction.CommitAsync();
//        }
//    }
//    catch (Exception ex)
//    {
//        return new ResponseMessage(500, false, ex.Message);
//    }

//    return new ResponseMessage(200, true, "Created Successfully");
//}