using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.Entities;
using EXAM_SYSTEM_API.Domain.Interfaces;

namespace EXAM_SYSTEM_API.Infrastructure.Persistence.Repositories
{
    public class UserSelectService : IUserSelectService
    {
        private readonly IServiceFactory _service;

        public UserSelectService(IServiceFactory service)
        {
            _service = service;
        }

        //// ดึงข้อสอบที่ทุกคนต้องสอบ พร้อมคำถาม (ฝั่งเจ้าหน้าที่ เผื่อต้องการดูข้อมูล) ***test***
        //public async Task<ResponseData> GetExampartWithQuestionAsync()
        //{
        //    var result = await _service.GetService<EsGeneratedExamQuestion>().GetAll()
        //                    .Where(x=> _service.GetService<EsGeneratedExamPart>().GetAll().Any(e => e.GeneratedPartId == x.GeneratedPartId)
        //                    && _service.GetService<EsQuestion>().GetAll().Any(q => q.QuestionId == x.QuestionId)).ToListAsync();

        //        //await _context.EsGeneratedExamQuestions
        //        //.Where(x => _context.EsGeneratedExamParts.Any(e => e.GeneratedPartId == x.GeneratedPartId)
        //        //&& _context.EsQuestions.Any(q=> q.QuestionId == x.QuestionId)).ToListAsync();

        //    return new ResponseData(200, true, "Get exampart with question successfully", result);
        //}

        // เลือกข้อสอบว่าจะทำข้อสอบชุดไหน กี่วัน วันที่เท่าไหร่บ้าง เพื่อที่จะนำไปคำนวนสร้างตาราง GenerateExamPart (ตารางไว้เก็บว่าทำข้อสอบวันที่เท่าไหร่บ้าง)
        public async Task<ResponseMessage> CreateAsync(EsExamUserSelection req,
            List<DateOnly> scheduledDate)
        {
            try
            {
                var examSet = await _service.GetService<EsExamSet>().GetByIdAsync(req.ExamsetId);
                if (examSet == null) return new ResponseMessage(200, false, "Examset not found");

                await _service.GetService<EsExamUserSelection>().AddAsync(req);
                //if (result.StatusCode != 200) return new ResponseMessage(400, false, "Create is failed");

                var EsGEPs = AddEsGEPs(req, examSet, SplitNumber(examSet.TotalQuestions, req.TotalDays), scheduledDate);
                await _service.GetService<EsGeneratedExamPart>().AddRangeAsync(EsGEPs);
            }
            catch (Exception ex)
            {
                return new ResponseMessage(500, false, ex.Message);
            }

            return new ResponseMessage(200, true, "Created Successfully");
        }

        private List<EsGeneratedExamPart> AddEsGEPs(EsExamUserSelection req, EsExamSet examSet, List<int> numquestion, List<DateOnly> scheduledDate)
        {
            var createdDate = DateTime.Now;

            return Enumerable.Range(0, req.TotalDays).Select(i => new EsGeneratedExamPart
            {
                SelectionId = req.SelectionId, // รหัสของรายการเลือก
                PartNumber = i + 1, // วันที่ของการสอบ
                NumQuestions = numquestion[i], // จำนวนข้อสอบใน Part นี้
                ScheduledDate = scheduledDate[i],  // วันทีการสอบ
                CreatedDate = createdDate,  // วันที่สร้าง
                PersonId = req.PersonId,
            }).ToList();
        }

        // ดูชุดข้อสอบของตัวเองที่สอบไปแล้ว
        public Task<List<dynamic>> GetResultExamSetByUserAsync(int userId)
        {
            var iqueryable = _service.GetService<EsExamFinalResult>().GetAll()
                                .Where(efr => efr.PersonId == userId)
                                .Join(
                                   _service.GetService<EsExamSet>().GetAll(),
                                    efr => efr.ExamsetId,
                                    es => es.ExamsetId,
                                    (efr, es) => new
                                    {
                                        es.ExamsetId,
                                        es.ExamsetName,
                                        es.NumDateExam,
                                        es.CreatedDate,
                                        es.Description,
                                        es.IsUsed,
                                        es.TotalQuestions,
                                        efr.TotalDays,
                                        efr.TotalScore,
                                        efr.Status,
                                    });

            //(from efr in _context.EsExamFinalResults
            //                  join es in _context.EsExamSets on efr.ExamsetId equals es.ExamsetId
            //                  where efr.PersonId == userId
            //                  select new
            //                  {
            //                      es.ExamsetId,
            //                      es.ExamsetName,
            //                      es.NumDateExam,
            //                      es.CreatedDate,
            //                      es.Description,
            //                      es.IsUsed,
            //                      es.TotalQuestions,
            //                      efr.TotalDays,
            //                      efr.TotalScore,
            //                      efr.Status,
            //                  });

            return iqueryable.ToListAsync().ContinueWith(task => task.Result.Cast<dynamic>().ToList());
        }

        // ดูชุดข้อสอบที่ตัวเองยังไม่ได้สอบ
        //public async Task<List<EsGeneratedExamPart>> GetExamWaitingByUserAsync(int userId)
        //{
        //    return await _service.GetService<EsGeneratedExamPart>().GetAll()
        //        .Where(x => !_service.GetService<EsExamTaken>().GetAll()
        //        .Any(c => c.GeneratedPartId == x.GeneratedPartId)).ToListAsync();

        //    //return await _context.EsGeneratedExamParts
        //    //    .Where(x => !_context.EsExamTakens.Any(c => c.GeneratedPartId == x.GeneratedPartId)).ToListAsync();
        //}


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

        //// test ทำเผื่อไว้ เผื่ออาจารย์ต้องการในอนาคต
        //// เปลี่ยนจาก EsExamTaken เป็นตารางอื่นที่อาจารย์ต้องการ หรือ เพิ่มตารางเข้ามาใหม่ สำหรับทำข้อสอบ
        //public async Task<ResponseData> NewCreateExamTaken(EsExamTaken req)
        //{
        //    try
        //    {
        //        await _service.GetService<EsExamTaken>().AddAsync(req);

        //        var enrm = await _service.GetService<EsExamEnrollment>().GetByIdAsync(req.SelectionId);
        //        var schedule = await _service.GetService<EsExamSchedule>().GetByIdAsync(req.GeneratedPartId);

        //        if (enrm is null) return new ResponseData(200, false, "Enrollment is null");
        //        if (schedule is null) return new ResponseData(200, false, "ExamSchedule is null");

        //        schedule.Status = 1;

        //        await _service.GetService<EsExamSchedule>().UpdateAsync(schedule);

        //        if (enrm.NumDays == schedule.PartNo)
        //        {
        //            var totalScore = await _service.GetService<EsExamTaken>().GetAll()
        //                .Where(et => et.SelectionId == req.SelectionId)
        //                .SumAsync(et => et.Score);

        //            var finalR = new EsExamFinalResult
        //            {
        //                PersonId = enrm.PersonId,
        //                ExamsetId = enrm.ExamsetId,
        //                TotalScore = totalScore,
        //                Status = "1",
        //                TotalDays = enrm.NumDays,
        //            };

        //            var result = await _service.GetService<EsExamFinalResult>().AddAsync(finalR);
        //            if (result.StatusCode != 200) return new ResponseData(400, false, "Create failed");

        //            return new ResponseData(200, true, "You have completed all the question sets.", finalR);
        //        }

        //        return new ResponseData(200, true, "Created successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseData(500, false, $"Unexpected error: {ex.Message}");
        //    }
        //}

    }
}

//public async Task<ResponseMessage> CreateAsync(EsExamUserSelection req,
//            List<DateOnly> scheduledDate)
//{
//    using (var transaction = await _context.Database.BeginTransactionAsync())
//    {
//        try
//        {
//            var examSet = await _examSetService.GetByIdAsync(req.ExamsetId);
//            if (examSet == null) return new ResponseMessage(200, false, "Examset not found");

//            await _selectService.AddAsync(req);
//            //if (result.StatusCode != 200) return new ResponseMessage(400, false, "Create is failed");

//            var EsGEPs = req.AddEsGEPs(req, examSet, scheduledDate);
//            await _examPartService.AddRangeAsync(EsGEPs);

//            await transaction.CommitAsync();
//        }
//        catch (Exception ex)
//        {
//            await transaction.RollbackAsync();
//            return new ResponseMessage(500, false, ex.Message);
//        }
//    }

//    return new ResponseMessage(200, true, "Created Successfully");
//}