using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;

namespace Core.DTOs.PersonalDTOs;

public class PersonalOffDayDto
{
    public string NameSurname { get; set; }
    public int TotalYearLeave { get; set; } 
    public int UsedYearLeave { get; set; } 
    public BranchDto Branch { get; set; }
    public PositionDto Position { get; set; }
    public int Branch_Id { get; set; }
}