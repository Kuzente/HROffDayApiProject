using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;

namespace Core.DTOs.PersonalDTOs;

public class PersonalDto : BaseDto
{
	public string NameSurname { get; set; }
	public DateTime StartJobDate { get; set; }
	public bool RetiredOrOld { get; set; }
	public string Gender { get; set; }
	public int RegistirationNumber { get; set; } 
	public Guid Branch_Id { get; set; }
	public Guid Position_Id { get; set; }
	public BranchNameDto Branch { get; set; }
	public PositionNameDto Position { get; set; }
	public int TotalYearLeave { get; set; } 
	public int UsedYearLeave { get; set; } 
	
	// public string? Phonenumber { get; set; }
	// public string IdentificationNumber { get; set; } 
	// public DateTime BirthDate { get; set; }


}