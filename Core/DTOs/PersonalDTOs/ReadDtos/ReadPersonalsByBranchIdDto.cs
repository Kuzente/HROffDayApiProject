namespace Core.DTOs.PersonalDTOs.ReadDtos;

public class ReadPersonalsByBranchIdDto
{
    public Guid ID { get; set; }
    public string NameSurname { get; set; }
    public int TotalYearLeave { get; set; }
    public int TotalTakenLeave { get; set; }
    public int UsedYearLeave { get; set; }
    public string BranchName { get; set; }
    public string PositionName { get; set; }
}