using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;

namespace Core.DTOs.PersonalDTOs;

public class PersonalDto : ReadBaseDto
{
		
	public string NameSurname { get; set; }
		
	public DateTime StartJobDate { get; set; }
	public bool RetiredOrOld { get; set; }
	
	public string Gender { get; set; }
	
	public string RegistirationNumber { get; set; } 
	public int Branch_Id { get; set; }
	public int Position_Id { get; set; }
	public BranchNameDto Branch { get; set; }
	public PositionNameDto Position { get; set; }
	public int TotalYearLeave { get; set; } 
	public int UsedYearLeave { get; set; } 
	
	// public string? Phonenumber { get; set; }
	// public string IdentificationNumber { get; set; } 
	// public DateTime BirthDate { get; set; }


}