namespace Core.DTOs.BaseDTOs;

public class FilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? SortOrder { get; set; }
    public string? SortColumn { get; set; }
}