using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;

namespace Core.DTOs.PersonalDTOs;

public class PersonalDetailDto : BaseDto
{
    public string NameSurname { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime StartJobDate { get; set; }
    public DateTime? EndJobDate { get; set; } 
    public string IdentificationNumber { get; set; }
    public string RegistirationNumber { get; set; }
    public string Phonenumber { get; set; }
    public bool RetiredOrOld { get; set; }
        
    public string Gender { get; set; }
    public int TotalYearLeave { get; set; }
    public int UsedYearLeave { get; set; } 
    public Guid Branch_Id { get; set; }
    public Guid Position_Id { get; set; }
    public List<BranchNameDto> Branches { get; set; }
    public List<PositionNameDto> Positions { get; set; }
    
}
