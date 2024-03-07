using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class DailyYearLog : BaseEntity
{
    [Required]
    public string NameSurname { get; set; }
    [Required]
    public int AddedYearLeave { get; set; }
    [Required]
    public string AddedYearLeaveDescription { get; set; }
    
}