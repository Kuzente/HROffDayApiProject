using Core.Entities;
using Core.Enums;

namespace Core.DTOs.OffDayDTOs.ReadDtos;

public class ReadApprovedOffDayListDto
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
    public ReadApprovedOffDayListSubPersonalDto Personal { get; set; }
}

public class ReadApprovedOffDayListSubPersonalDto
{
    public Guid ID { get; set; }
    public string NameSurname { get; set; }
    public ReadApprovedOffDayListSubPersonalBranchDto Branch { get; set; }
    public ReadApprovedOffDayListSubPersonalPositionDto Position { get; set; }
}

public class ReadApprovedOffDayListSubPersonalBranchDto
{
    public string Name { get; set; }
}
public class ReadApprovedOffDayListSubPersonalPositionDto
{
    public string Name { get; set; }
}