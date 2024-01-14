using Core.DTOs.BaseDTOs;

namespace Core.DTOs.PersonalDTOs;

public class WritePersonalDto : BaseDto
{
	public string NameSurname { get; set; }
	public DateTime BirthDate { get; set; }
	public string IdentificationNumber { get; set; }
	public string RegistirationNumber { get; set; }
	public Guid Branch_Id { get; set; }
	public Guid Position_Id { get; set; }
	
	public string Gender { get; set; }
}