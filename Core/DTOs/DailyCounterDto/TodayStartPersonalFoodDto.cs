namespace Core.DTOs.DailyCounterDto;

public class TodayStartPersonalFoodDto
{
    public string NameSurname { get; set; }
    public string AddedFoodAidAmountDescription { get; set; }
    public int AddedFoodAidAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}