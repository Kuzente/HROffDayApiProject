using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class DailyCounter : BaseEntity
{
    [Required]
    public string NameSurname { get; set; }
    [Required]
    public int AddedYearLeave { get; set; }
    [Required]
    public double AddedFoodAidAmount { get; set; }
    [Required]
    public string AddedYearLeaveDescription { get; set; }
    [Required]
    public string AddedFoodAidAmountDescription { get; set; }
}