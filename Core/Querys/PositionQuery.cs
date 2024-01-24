namespace Core.Querys;

public class PositionQuery
{
    public string search { get; set; } 
    public int sayfa { get; set; } = 1;
    public bool? passive { get; set; } 
    public bool? active { get; set; } 
}