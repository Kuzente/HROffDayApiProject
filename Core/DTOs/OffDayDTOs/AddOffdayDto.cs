using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PersonalDTOs;
using Core.DTOs.PositionDTOs;
using Core.Entities;
using Core.Enums;

namespace Core.DTOs.OffDayDTOs;

public class AddOffdayDto : BaseDto
{
    public Guid Personal_Id { get; set; }
    
    public PersonalOffDayDto Personal { get; set; }
    
    public string BranchName { get; set; }
    public string PositionName { get; set; }
    public List<PersonalOffDayDto> PersonalOffDayDtos{ get; set; }
    public DateTime? EndDate { get; set; } 
    public DateTime? StartDate { get; set; }
    public int LeaveByWeek { get; set; }
    public int LeaveByYear { get; set; } 
    public int LeaveByPublicHoliday { get; set; } 
    public int LeaveByFreeDay { get; set; } 
    public int LeaveByTaken { get; set; } 
    public int LeaveByTravel { get; set; } 
    public int LeaveByDead { get; set; } 
    public int LeaveByFather { get; set; }
    public int LeaveByMarried { get; set; }
    public string? Description { get; set; }
}