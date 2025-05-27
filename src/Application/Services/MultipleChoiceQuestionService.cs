using System.Text.Json;
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
    public class MultipleChoiceQuestionService : IMultipleChoiceQuestionService
    {
        private readonly IServiceFactory _service;

        public MultipleChoiceQuestionService(IServiceFactory service)
        {
            _service = service;
        }

        public class ChoiceItem
        {
            public int Id { get; set; }
            public string Anwer { get; set; } = string.Empty;
        }

        public async Task<ResponseData> GetRandomAsk(int enrollmentId, int scheduleId)
        {

            try
            {
                if (enrollmentId <= 0)
                {
                    return new ResponseData(400, false, "รหัสการลงทะเบียนไม่ถูกต้อง", null);
                }

                var Enrollment = await _service.GetService<EsExamEnrollment>().GetAll().Where(a => a.EnrollmentId == enrollmentId).FirstOrDefaultAsync();

                var Schedules = await _service.GetService<EsExamSchedule>()
                                                .GetAll()
                                                .Where(a => a.EnrollmentId == enrollmentId)
                                                .ToListAsync();

                var _Schedule = Schedules.Where(a => (scheduleId == 0) || a.ScheduleId == scheduleId).FirstOrDefault();

                if (_Schedule == null)
                {
                    return new ResponseData(404, false, "ไม่พบนัดสอบสำหรับรหัสนี้", null);
                }

                var ScheduleValues = Schedules.Select(s => s.ScheduleId).ToList();

                //ดึงข้อมูลที่ทำแล้ว
                var _Taken = await _service.GetService<EsExamTaken>()
                                            .GetAll()
                                            .Where(a => a.ScheduleId != scheduleId)
                                            .Where(a => ScheduleValues.Contains(a.ScheduleId))
                                            .Select(a => a.ExamTakenId)
                                            .ToListAsync();

                var _TakenAnswer = await _service.GetService<EsExamTakenDetail>()
                                                .GetAll()
                                                .Where(a => _Taken.Contains(a.ExamTakenId))
                                                .Select(a => a.ChoiceAnswer)
                                                .ToListAsync();

                var _lists = new List<ChoiceItem>();
                foreach (var _Answer in _TakenAnswer)
                {
                    if (!string.IsNullOrWhiteSpace(_Answer))
                    {
                        try
                        {
                            var choices = JsonSerializer.Deserialize<List<ChoiceItem>>(_Answer);
                            if (choices != null)
                            {
                                _lists.AddRange(choices);
                            }
                        }
                        catch (JsonException ex)
                        {

                        }
                    }
                }

                var anwerValues = _lists.Select(c => c.Id).ToList();
                //ดึงข้อมูลสอบ
                var Questions = await _service.GetService<EsMultipleChoiceQuestion>()
                                            .GetAll()
                                            .Where(a => !anwerValues.Contains(a.QuestionId))
                                            .Select(a => new
                                            {
                                                Id = a.QuestionId,
                                                Question = a.QuestionText,
                                                Options = new[] { a.ChoiceA, a.ChoiceB, a.ChoiceC, a.ChoiceD },
                                                Anwer = a.CorrectAnswer
                                            })
                                            .Take(_Schedule.TotalQuestions)
                                            .OrderBy(x => Guid.NewGuid())
                                            .ToListAsync();

                TimeOnly startTime = _Schedule.StartTime;
                TimeOnly endTime = _Schedule.EndTime;
                double diffInSeconds = (endTime - startTime).TotalSeconds;
                var result = new
                {
                    Time = diffInSeconds,
                    Question = Questions
                };

                return new ResponseData(200, true, "สำเร็จ", result);
            }
            catch (Exception ex)
            {
                // Log the exception here if you have a logging system
                return new ResponseData(500, false, $"เกิดข้อผิดพลาดภายในระบบ: {ex.Message} {ex.InnerException?.Message}", null);
            }
        }

        public async Task<ResponseMessage> SaveExamQuiz(ExamQuizReq req)
        {

            try
            {

                var EnrollmentId = req.EnrollmentId;
                var ScheduleId = req.ScheduleId;

                TimeOnly startedTime = string.IsNullOrEmpty(req.StartedTime.ToString()) ? TimeOnly.FromTimeSpan(DateTime.UtcNow.TimeOfDay) : TimeOnly.FromTimeSpan(req.StartedTime.TimeOfDay);
                TimeOnly SubmittedTime = string.IsNullOrEmpty(req.SubmittedTime.ToString()) ? TimeOnly.FromTimeSpan(DateTime.UtcNow.TimeOfDay) : TimeOnly.FromTimeSpan(req.SubmittedTime.TimeOfDay);
                EsExamTaken? _Taken = null;

                _Taken = await _service.GetService<EsExamTaken>().GetAll().Where(a => a.ScheduleId == ScheduleId).FirstOrDefaultAsync();

                if (_Taken == null)
                {
                    _Taken = new EsExamTaken
                    {
                        ScheduleId = ScheduleId,
                        StartedTime = startedTime,
                        SubmittedTime = SubmittedTime,
                        Status = null,
                        Score = null
                    };
                    await _service.GetService<EsExamTaken>().AddAsync(_Taken);
                }
                else
                {
                    _Taken.StartedTime = startedTime;
                    _Taken.SubmittedTime = SubmittedTime;
                }

                var quizValues = req.ListQuiz.Select(c => c.Id).ToList();
                var Questions = await _service.GetService<EsMultipleChoiceQuestion>()
                            .GetAll()
                            .Where(a => quizValues.Contains(a.QuestionId))
                            .Select(a => new
                            {
                                Id = a.QuestionId,
                                Answer = a.CorrectAnswer
                            })
                            .ToListAsync();

                var _lists = new List<int>();

                foreach (var _item in req.ListQuiz)
                {
                    var checkedVal = Questions.Where(a => a.Id == _item.Id && a.Answer == _item.Answer).FirstOrDefault();
                    if (checkedVal != null)
                    {
                        _lists.Add(_item.Id);
                    }
                }

                _Taken.Score = _lists.Count();
                _Taken.Status = "1";
                await _service.GetService<EsExamTaken>().UpdateAsync(_Taken);

                var _TakenDetail = await _service.GetService<EsExamTakenDetail>()
                                                .GetAll()
                                                .Where(a => a.ExamTakenId == _Taken.ExamTakenId)
                                                .FirstOrDefaultAsync();

                if (_TakenDetail == null)
                {

                    _TakenDetail = new EsExamTakenDetail
                    {
                        ExamTakenId = _Taken.ExamTakenId,
                        ChoiceAnswer = (string)JsonSerializer.Serialize(req.ListQuiz),
                        NumCorrect = _lists.Count(),
                        NumIncorrect = (req.ListQuiz.Count() - _lists.Count())
                    };

                    await _service.GetService<EsExamTakenDetail>().AddAsync(_TakenDetail);
                }
                else
                {
                    _TakenDetail.ChoiceAnswer = (string)JsonSerializer.Serialize(req.ListQuiz);
                    _TakenDetail.NumCorrect = _lists.Count();
                    _TakenDetail.NumIncorrect = (req.ListQuiz.Count() - _lists.Count());
                    await _service.GetService<EsExamTakenDetail>().UpdateAsync(_TakenDetail);
                }

                var _Schedule = await _service.GetService<EsExamSchedule>().GetAll().Where(a => a.ScheduleId == ScheduleId).FirstOrDefaultAsync();
                if (_Schedule != null)
                {
                    _Schedule.Status = "1";
                    await _service.GetService<EsExamSchedule>().UpdateAsync(_Schedule);
                }

                return new ResponseMessage(200, true, $"สำเร็จ");

            }
            catch (Exception ex)
            {
                return new ResponseMessage(500, false, $"ไม่สำเร็จ :  {ex.Message} {ex.InnerException?.Message}");
            }

        }

        public async Task<ResponseData> ReportUocExamQuiz(int? enrollmentId, int? scheduleId)
        {

            if (enrollmentId <= 0)
            {
                return new ResponseData(400, false, "รหัสการลงทะเบียนไม่ถูกต้อง", null);
            }

            if (scheduleId <= 0)
            {
                return new ResponseData(400, false, "ไม่พบนัดสอบสำหรับรหัสนี้", null);
            }

            var _Schedule = await _service.GetService<EsExamSchedule>().GetAll().Where(a => a.ScheduleId == scheduleId).FirstOrDefaultAsync();
            var _Uoc = await _service.GetService<VEsExamTakenUocSummary>().GetAll().Where(a => a.ScheduleId == scheduleId && a.EnrollmentId == enrollmentId).ToListAsync();

            var result = new
            {
                Schedule = _Schedule,
                Uoc = _Uoc
            };
            return new ResponseData(200, true, "สำเร็จ", result);
        }

        public async Task<PaginationResponse<VEsExamScheduleDetail>> GetAllExamHistory(ExamHistoryReq req)
        {

            var PersonId = req.PersonId;
            var query = await _service.GetService<VEsExamScheduleDetail>()
                                    .GetAll()
                                    .Where(a => a.ExamTakenId != null)
                                    .Where(a => (PersonId == 0) || (a.PersonId == PersonId))
                                    .OrderByDescending(a => a.CreatedDate)
                                    .ToListAsync();

            return new PaginationResponse<VEsExamScheduleDetail>(query, query.Count, req.CurrentPage, req.PageSize);
        }


    }
}


