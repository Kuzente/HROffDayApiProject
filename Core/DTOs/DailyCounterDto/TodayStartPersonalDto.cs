namespace Core.DTOs.DailyCounterDto;

public class TodayStartPersonalDto
{
    public string NameSurname { get; set; }
    public string AddedYearLeaveDescription { get; set; }
    public string AddedFoodAidAmountDescription { get; set; }
    public int AddedYearLeave { get; set; }
    public double AddedFoodAidAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}