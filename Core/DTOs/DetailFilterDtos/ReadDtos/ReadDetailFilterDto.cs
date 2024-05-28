namespace Core.DTOs.DetailFilterDtos.ReadDtos;

public class ReadDetailFilterDto
{
    public List<ReadFilterDto> Filters { get; set; }
    public string EntityName { get; set; }
}

public class ReadFilterDto
{
    public string Property { get; set; }
    public string Type { get; set; }
    public string SearchValue { get; set; }
    public List<string> SelectedValues { get; set; }
    public string Comparison { get; set; }
    public List<double> Amounts { get; set; }
    public List<DateTime> Dates { get; set; }
    public bool CheckedValue { get; set; }
}