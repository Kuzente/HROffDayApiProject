using Core.Enums;

namespace Core.DTOs.PersonalDetailDto.ReadDtos;

public class ReadPersonalDetailsHeaderDto
{
    public Guid ID { get; set; }
    public string NameSurname { get; set; }
    public string IdentificationNumber { get; set; }
    public int TotalYearLeave { get; set; }
    public double TotalTakenLeave { get; set; }
    public int UsedYearLeave { get; set; }
    public int FoodAid { get; set; }
    public DateTime EndJobDate { get; set; }
    public DateTime StartJobDate { get; set; }
    public EntityStatusEnum Status { get; set; }
    public bool IsBackToWork { get; set; }
    public ReadPersonalDetailsHeaderSubBranchDto Branch { get; set; }
    public ReadPersonalDetailsHeaderSubPositionDto Position { get; set; }
}

public class ReadPersonalDetailsHeaderSubBranchDto
{
    public string Name { get; set; }
}
public class ReadPersonalDetailsHeaderSubPositionDto
{
    public string Name { get; set; }
}