namespace Core.Querys;

public class MissingDayQuery
{
    public Guid? id { get; set; }
    public string search { get; set; } 
    public string sortName { get; set; }
    public string sortBy { get; set; }
    public int? filterYear { get; set; }
    public int? filterMonth { get; set; }
    public int sayfa { get; set; } = 1;
}