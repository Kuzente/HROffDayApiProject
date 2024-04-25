using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public partial class PersonalCumulative : BaseEntity
{
    public int Year { get; set; }
    public int EarnedYearLeave { get; set; }
    public int RemainYearLeave { get; set; }
    public bool IsReportCompleted { get; set; }
    public bool IsNotificationExist { get; set; }
}
public partial class PersonalCumulative
{
    public Guid Personal_Id { get; set; }
    [ForeignKey(nameof(Personal_Id))]
    public Personal Personal { get; set; }
    
}