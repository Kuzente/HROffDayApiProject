namespace Core.DTOs.PersonalCumulativeDtos.ReadDtos;

public class ReadUpdatePersonalCumulativeDto
{
    public Guid ID { get; set; }
    public int Year { get; set; }
    public int EarnedYearLeave { get; set; }
    public int RemainYearLeave { get; set; }
    public bool IsReportCompleted { get; set; }
    public bool IsNotificationExist { get; set; }
    public Guid Personal_Id { get; set; }
}