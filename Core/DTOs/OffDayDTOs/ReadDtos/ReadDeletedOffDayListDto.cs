﻿using Core.Enums;

namespace Core.DTOs.OffDayDTOs.ReadDtos;

public class ReadDeletedOffDayListDto
{
    public Guid ID { get; set; }
    public DateTime DeletedAt { get; set; }
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
    public Guid BranchId { get; set; }
    public Guid PositionId { get; set; }
    public string BranchName { get; set; }
    public string PositionName { get; set; }
    public ReadDeletedOffDayListSubPersonalDto Personal { get; set; }
}
public class ReadDeletedOffDayListSubPersonalDto
{
    public Guid ID { get; set; }
    public string NameSurname { get; set; }
}

