using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;

namespace Core.DTOs.PersonalDTOs;

public class PersonalOffDayDto : BaseDto
{
    public string NameSurname { get; set; }
    public int TotalYearLeave { get; set; }
    public int UsedYearLeave { get; set; } 
    public BranchNameDto Branch { get; set; }
    public PositionNameDto Position { get; set; }
    
}