namespace Core.Querys;

public class OffdayQuery
{
    public string search { get; set; } 
    public int sayfa { get; set; } = 1;
    public int? filterYear { get; set; }
    public int? filterMonth { get; set; }
    public string positionName { get; set; }
    public string branchName { get; set; }
    public Guid? id { get; set; }
    
}