using Core.Enums;

namespace Core.Querys;

public class BranchQuery
{
    public string search { get; set; } 
    public int sayfa { get; set; } = 1;
    public string isActive { get; set; }
    public string sortName { get; set; }
    public string sortBy { get; set; }
    
}