using Core.Entities;
using Core.Enums;

namespace Core.DTOs.OffDayDTOs.ReadDtos;

public class ReadApprovedOffDayDto
{
    public List<ReadApprovedOffDayListDto> ReadApprovedOffDayListDtos { get; set; }
    public List<ReadApprovedOffDayGetBranches> ReadApprovedOffDayGetBranchesList { get; set; }
    public List<ReadApprovedOffDayGetPositions> ReadApprovedOffDayGetPositionsList { get; set; }
}
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
    public Guid BranchId { get; set; }
    public Guid PositionId { get; set; }
    public string BranchName { get; set; }
    public string PositionName { get; set; }
    public int DocumentNumber { get; set; }
    public ReadApprovedOffDayListSubPersonalDto Personal { get; set; }
}

public class ReadApprovedOffDayListSubPersonalDto
{
    public Guid ID { get; set; }
    public string NameSurname { get; set; }
    public EntityStatusEnum Status { get; set; }
    public bool IsBackToWork { get; set; }
}

