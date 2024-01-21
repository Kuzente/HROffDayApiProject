using Core.Enums;

namespace Core.DTOs.PersonalDetailDto.ReadDtos;

public class ReadPersonalDetailsHeaderDto
{
    public Guid ID { get; set; }
    public string NameSurname { get; set; }
    public int TotalYearLeave { get; set; }
    public int TotalTakenLeave { get; set; }
    public int UsedYearLeave { get; set; }
    public EntityStatusEnum Status { get; set; }
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