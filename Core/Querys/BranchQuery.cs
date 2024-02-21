using Core.Enums;

namespace Core.Querys;

public class BranchQuery
{
    public string search { get; set; } 
    public int sayfa { get; set; } = 1;
    public bool? passive { get; set; }
    public bool? active { get; set; }
    
}