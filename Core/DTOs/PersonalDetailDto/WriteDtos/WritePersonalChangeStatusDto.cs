namespace Core.DTOs.PersonalDetailDto.WriteDtos;

public class WritePersonalChangeStatusDto
{
    public Guid ID { get; set; }
    public DateTime EndJobDate { get; set; }
    public DateTime StartJobDate { get; set; }
    public bool IsYearLeaveProtected { get; set; }
    public bool IsTakenLeaveProtected { get; set; }
    public bool IsFoodAidProtected { get; set; }
    public bool IsYearLeaveDateProtected { get; set; }
    public DateTime FoodAidDate { get; set; }
}