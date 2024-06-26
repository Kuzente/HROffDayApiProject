﻿using Core.Entities;
using Core.Enums;

namespace Core.DTOs.OffDayDTOs.ReadDtos;

public class ReadWaitingOffDayEditDto
{
    public Guid ID { get; set; }
    public DateTime CreatedAt { get; set; }
    public OffDayStatusEnum OffDayStatus { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CountLeave { get; set; }
    public int LeaveByWeek { get; set; }
    public int LeaveByYear { get; set; } 
    public int LeaveByPublicHoliday { get; set; } 
    public int LeaveByFreeDay { get; set; } 
    public int LeaveByTaken { get; set; } 
    public int LeaveByTravel { get; set; } 
    public int LeaveByDead { get; set; } 
    public int LeaveByFather { get; set; }
    public int LeaveByMarried { get; set; }
    public Guid Personal_Id { get; set; }
    public string PositionName { get; set; }
    public string BranchName { get; set; }
    public Guid BranchId { get; set; }
    public Guid PositionId { get; set; }
    public string? HrName { get; set; }
    public string? DirectorName { get; set; }
    public ReadWaitingOffDayEditSubPersonalDto Personal { get; set; }
}

public class ReadWaitingOffDayEditSubPersonalDto
{
    public Guid ID { get; set; }
    public string NameSurname { get; set; }
    public int TotalYearLeave { get; set; }
    public int UsedYearLeave { get; set; }
    public double TotalTakenLeave { get; set; }
}
