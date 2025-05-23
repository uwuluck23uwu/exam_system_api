﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EXAM_SYSTEM_API.Domain.Entities;

public partial class EsExamUserSelection
{
    /// <summary>
    /// รหัสของรายการเลือก
    /// </summary>
    public int SelectionId { get; set; }

    /// <summary>
    /// รหัสผู้สอบ
    /// </summary>
    public int PersonId { get; set; }

    /// <summary>
    /// รหัสชุดข้อสอบ
    /// </summary>
    public int ExamsetId { get; set; }

    /// <summary>
    /// สถานะการสอบ
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// จำนวนวันสอบที่เลือก (1-5 วัน)
    /// </summary>
    public int TotalDays { get; set; }

    /// <summary>
    /// วันที่สร้าง
    /// </summary>
    public DateTime? CreatedDate { get; set; }

    /// <summary>
    /// วันที่สอบเสร็จ
    /// </summary>
    public DateTime? CompletedAt { get; set; }
}