using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Shared;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.CustomResponse;
using EXAM_SYSTEM_API.Domain.Entities;

namespace EXAM_SYSTEM_API.Application.Services
{
    public class ExamScheduleService : IExamScheduleService
    {
        private readonly IServiceFactory _service;
        private readonly CultureInfoTHEN _cultureInfoTHEN;
        private readonly ConvertDate _convertDate;

        public ExamScheduleService(IServiceFactory service)
        {
            _service = service;
            _cultureInfoTHEN = new CultureInfoTHEN();
            _convertDate = new ConvertDate();
        }


        public async Task<PaginationResponse<ExamEnrollmentGroupResponse>> GetMyExamEnrollmentAsync(
            int personId, int examSetType, int pageSize, int currentPage)
        {
            // ดึงชุดข้อสอบตามประเภท
            var examSets = await _service.GetService<EsExamSet>()
                .GetAll()
                .Where(x => x.ExamsetType == examSetType)
                .ToListAsync();

            var examSetIds = examSets.Select(x => x.ExamsetId).ToList();

            // ดึงการสมัครสอบตาม personId และประเภทข้อสอบ
            var enrollmentsList = await _service.GetService<EsExamEnrollment>()
                .GetAll()
                .Where(e => e.PersonId == personId && examSetIds.Contains(e.ExamsetId))
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();

            var enrollmentIds = enrollmentsList.Select(e => e.EnrollmentId).ToList();

            // ดึงตารางสอบทั้งหมดที่เกี่ยวข้อง
            var allSchedules = await _service.GetService<EsExamSchedule>()
                .GetAll()
                .Where(s => enrollmentIds.Contains(s.EnrollmentId))
                .ToListAsync();

            var currentDate = DateTime.Now;

            foreach (var item in allSchedules)
            {
                var endDateTime = item.ScheduleDate.ToDateTime(item.EndTime);

                if (endDateTime < currentDate)
                {
                    if (item.Status != "1")
                    {
                        item.Status = "2";
                    }
                }
            }

            await _service.GetService<EsExamSchedule>()
                .UpdateRangeAsync(allSchedules);

            // เรียง schedules ตามลำดับ enrollmentIds
            var scheduleLookup = allSchedules.ToLookup(s => s.EnrollmentId);
            var sortedSchedules = enrollmentIds
                .SelectMany(id => scheduleLookup[id])
                .ToList();

            // ตัดตารางสอบตามหน้าที่ขอ
            var schedulesPage = sortedSchedules
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var selectedEnrollmentIds = schedulesPage
                .Select(s => s.EnrollmentId)
                .Distinct()
                .ToList();

            // สร้าง dictionary สำหรับ ExamSet
            var examSetMap = examSets.ToDictionary(x => x.ExamsetId, x => new ExamSetResponse
            {
                ExamsetId = x.ExamsetId,
                ExamsetType = x.ExamsetType,
                ExamsetName = x.ExamsetName,
                StdId = x.StdId,
                StdName = x.StdName,
                Description = x.Description,
                TotalQuestions = x.TotalQuestions,
                MaxScore = x.MaxScore,
                PassPercentage = x.PassPercentage,
                NumDateExam = x.NumDateExam,
                IsUsed = x.IsUsed,
            });

            // ดึงเฉพาะ enrollment ที่เกี่ยวข้องกับ schedules ในหน้านี้
            var selectedEnrollments = enrollmentsList
                .Where(e => selectedEnrollmentIds.Contains(e.EnrollmentId))
                .ToList();

            // จัดกลุ่มและรวมข้อมูลสำหรับ response
            var grouped = selectedEnrollments
                .GroupBy(e => e.CreatedDate.Date)
                .Select(g => new ExamEnrollmentGroupResponse
                {
                    CreatedDate = _cultureInfoTHEN.FormatDateTH(g.Key, "dd/MM/yyyy TH "),
                    ExamEnrollments = g.Select(e => new ExamEnrollmentWithExamScheduleResponse
                    {
                        EnrollmentId = e.EnrollmentId,
                        NumDays = e.NumDays,
                        ExamSet = examSetMap[e.ExamsetId],
                        ExamSchedules = schedulesPage
                            .Where(s => s.EnrollmentId == e.EnrollmentId)
                            .OrderBy(s => s.PartNo)
                            .Select(s => new ExamScheduleResponse
                            {
                                ScheduleId = s.ScheduleId,
                                PartNo = s.PartNo,
                                ScheduleDate = _cultureInfoTHEN.FormatDateTH(_convertDate.DateOnlyToDateTime(s.ScheduleDate), "dd/MM/yyyy TH "),
                                StartTime = s.StartTime,
                                EndTime = s.EndTime,
                                TotalQuestions = s.TotalQuestions,
                                Status = s.Status,
                                StatusName = s.Status switch
                                {
                                    "0" => "ยังไม่ได้สอบ",
                                    "1" => "สอบแล้ว",
                                    "2" => "เลื่อนเวลาสอบ",
                                    _ => "ไม่ได้สอบ"
                                }
                            }).ToList()
                    }).ToList()
                })
                .ToList();

            return new PaginationResponse<ExamEnrollmentGroupResponse>(grouped, sortedSchedules.Count, currentPage, pageSize);
        }


        public async Task<ResponseData> GetEnrollmentByScheduleIdAsync(int scheduleId)
        {
            var examSchedule = await _service.GetService<EsExamSchedule>()
                .GetByIdAsync(scheduleId);

            if (examSchedule is null) return new ResponseData(200, false, "examSchedule not found");

            var enrollment = await _service.GetService<EsExamEnrollment>()
                .GetAll().FirstOrDefaultAsync(x => x.EnrollmentId.Equals(examSchedule.EnrollmentId));

            if (enrollment is null) return new ResponseData(200, false, "enrollment not found");

            return new ResponseData(200, true, "success", enrollment);
        }

        public async Task<ResponseMessage> CreateMyExamEnrollmentAsync(ExamEnrollmentRequest req)
        {
            try
            {
                var enr = await _service.GetService<EsExamEnrollment>().AddAsync(req.ExamEnrollment);
                var examSet = await _service.GetService<EsExamSet>().GetByIdAsync(req.ExamEnrollment.ExamsetId);

                if (examSet is null) return new ResponseMessage(200, false, "ExamSet is not found");
                if (req.ExamEnrollment.NumDays > examSet.NumDateExam) return new ResponseMessage(200, false, "คุณระบุจำนวนวันสอบเกินกว่าที่แนวทางปฏิบัติในการสอบระบุไว้");

                var totalQuestion = SplitNumber(examSet.TotalQuestions, req.ExamEnrollment.NumDays);

                var schedule = Enumerable.Range(0, req.ExamEnrollment.NumDays).Select(i => new EsExamSchedule
                {
                    EnrollmentId = req.ExamEnrollment.EnrollmentId,
                    PartNo = i + 1,
                    ScheduleDate = req.ExamSchedule[i].ScheduleDate,
                    StartTime = req.ExamSchedule[i].ScheduleStartTime,
                    EndTime = req.ExamSchedule[i].ScheduleEndTime,
                    TotalQuestions = totalQuestion[i],
                    Status = "0",
                    CreatedDate = DateTime.Now,
                }).ToList();

                await _service.GetService<EsExamSchedule>().AddRangeAsync(schedule);

                return new ResponseMessage(200, true, "Created Successfully");
            }
            catch (Exception ex)
            {
                return new ResponseMessage(500, false, ex.Message);
            }
        }

        // หารจำนวนข้อสอบ ในแต่ละส่วน
        private List<int> SplitNumber(int total, int parts)
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

            return split;
        }

        public async Task<ResponseMessage> UpdateExamSchedule(ExamReschedulesRequest req)
        {
            try
            {

                var reSchedule = await _service.GetService<EsExamSchedule>().GetAll().Where(a => a.ScheduleId == req.ScheduleId).FirstOrDefaultAsync();
                if (reSchedule == null)
                {
                    return new ResponseMessage(400, false, "ไม่พบตารางสอบของท่านนี้");
                }

                // log Change
                var _ReSchedule = new EsExamReschedule
                {
                    ScheduleId = reSchedule.ScheduleId,
                    ScheduleDate = reSchedule.ScheduleDate,
                    StartTime = reSchedule.StartTime,
                    EndTime = reSchedule.EndTime,
                    Reason = req.Reason,
                    CreatedDate = DateTime.Now
                };
                await _service.GetService<EsExamReschedule>().AddAsync(_ReSchedule);

                //Update
                reSchedule.ScheduleDate = req.ScheduleDate;
                reSchedule.StartTime = req.StartTime;
                reSchedule.EndTime = req.EndTime;
                reSchedule.Status = "2";
                await _service.GetService<EsExamSchedule>().UpdateAsync(reSchedule);

                return new ResponseMessage(200, true, $"สำเร็จ");
            }
            catch (Exception ex)
            {
                return new ResponseMessage(500, false, $"ไม่สำเร็จ :  {ex.Message} {ex.InnerException?.Message}");
            }

        }

    }
}