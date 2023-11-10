namespace Core.DTOs.PersonalDTOs;

public class AddRangePersonalDto
{
    public string NameSurname { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime StartJobDate { get; set; }
    public string IdentificationNumber { get; set; }
    public string RegistirationNumber { get; set; }
    public bool RetiredOrOld { get; set; }
    public string Gender { get; set; }
    public int TotalYearLeave { get; set; }
    public int UsedYearLeave { get; set; } 
    public int Branch_Id { get; set; }
    public int Position_Id { get; set; }
}