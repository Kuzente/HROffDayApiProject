using Core.Entities;
using Core.Enums;

namespace Core.DTOs.OffDayDTOs.ReadDtos;

public class ReadWaitingOffDayListDto
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
    public ReadWaitingOffDayListPersonalDto Personal { get; set; }
}

public class ReadWaitingOffDayListPersonalDto
{
    public string NameSurname { get; set; }
    public ReadWaitingOffDayListPersonalBranchDto Branch { get; set; }
    public ReadWaitingOffDayListPersonalPositionDto Position { get; set; }
}

public class ReadWaitingOffDayListPersonalBranchDto
{
    public string Name { get; set; }
}
public class ReadWaitingOffDayListPersonalPositionDto
{
    public string Name { get; set; }
}