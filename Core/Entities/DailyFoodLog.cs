using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class DailyFoodLog : BaseEntity
{
    [Required]
    public string NameSurname { get; set; }
    [Required]
    public double AddedFoodAidAmount { get; set; }
    [Required]
    public string AddedFoodAidAmountDescription { get; set; }
}