﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EXAM_SYSTEM_API.Domain.Entities;

public partial class ActivityLog
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; }

    public DateTime? Timestamp { get; set; }
}