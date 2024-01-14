namespace Core.DTOs.FilterDTOs;

public class PersonalFilterDto
{
    public string BranchName { get; set; }
    public Guid Branch_Id { get; set; }
    public string PositionName { get; set; }
    public Guid Position_Id { get; set; }
    public string Gender { get; set; }
    public bool Retired { get; set; }
}