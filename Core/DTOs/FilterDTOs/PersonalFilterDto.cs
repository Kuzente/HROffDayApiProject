namespace Core.DTOs.FilterDTOs;

public class PersonalFilterDto
{
    public string BranchName { get; set; }
    public int Branch_Id { get; set; }
    public string PositionName { get; set; }
    public int Position_Id { get; set; }
    public string Gender { get; set; }
    public bool Retired { get; set; }
}