﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EXAM_SYSTEM_API.Domain.Entities;

public partial class EsExamInstruction
{
    /// <summary>
    /// รหัสคำชี้แจง
    /// </summary>
    public int InstructionId { get; set; }

    /// <summary>
    /// รหัสชุดข้อสอบ
    /// </summary>
    public int ExamsetId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public bool ShowBeforeExam { get; set; }

    /// <summary>
    /// วันที่สร้างรายการข้อมูล
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ผู้สร้างรายการข้อมูล
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// วันที่ปรับปรุงรายการข้อมูล
    /// </summary>
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// ผู้ปรับปรุงรายการข้อมูล
    /// </summary>
    public string UpdatedBy { get; set; }
}