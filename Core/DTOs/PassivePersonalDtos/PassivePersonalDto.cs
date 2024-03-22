using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;

namespace Core.DTOs.PassivePersonalDtos;

public class PassivePersonalDto : BaseDto
{
    public string NameSurname { get; set; }
		
    public DateTime StartJobDate { get; set; }
    public DateTime? EndJobDate { get; set; } 
    public bool RetiredOrOld { get; set; }
	
    public string Gender { get; set; }
	
    public int RegistirationNumber { get; set; } 
    public Guid Branch_Id { get; set; }
    public Guid Position_Id { get; set; }
    public BranchNameDto Branch { get; set; }
    public PositionNameDto Position { get; set; }
    public int TotalYearLeave { get; set; } 
    public int UsedYearLeave { get; set; }
    public bool IsBackToWork { get; set; }
}

