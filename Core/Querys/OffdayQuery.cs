namespace Core.Querys;

public class OffdayQuery
{
    public string search { get; set; } 
    public int sayfa { get; set; } = 1;
	public string filterDate { get; set; }
	public string positionName { get; set; }
    public string branchName { get; set; }
    public string isFreedayLeave { get; set; }
    public Guid? id { get; set; }
    public string sortName { get; set; }
    public string sortBy { get; set; }
    public List<Guid> UserBranches { get; set; }
    
}