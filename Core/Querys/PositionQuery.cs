namespace Core.Querys;

public class PositionQuery
{
    public string search { get; set; } 
    public int sayfa { get; set; } = 1;
    public string isActive { get; set; }
    public string sortName { get; set; }
    public string sortBy { get; set; }
}