namespace Core.DTOs.DetailFilterDtos.ReadDtos;

public class ReadDetailFilterDto
{
    public List<ReadFilterDto>? Filters { get; set; }
    public string EntityName { get; set; }
    public bool IsValid(out List<string> errors)
    {
        errors = new List<string>();
        if (Filters != null)
            foreach (var filter in Filters)
            {
                if (filter.Type == "Text" && string.IsNullOrEmpty(filter.SearchValue))
                {
                    errors.Add($"{filter.PropertyName} alanı boş bırakılamaz!");
                }
                else if (filter.Type == "List" && (filter.SelectedValues is null || filter.SelectedValues.Count == 0))
                {
                    errors.Add($"{filter.PropertyName} alanı boş bırakılamaz!"); 
                }
                else if ((filter.Type == "Number" || filter.Type == "Double"))
                {
                    if (filter.Comparison is null ||filter.Amounts == null || filter.Amounts.Count == 0)
                    {
                        errors.Add($"{filter.PropertyName} için filtrenin boş olmadığından emin olunuz!");
                    }
                    else
                    {
                        foreach (var amount in filter.Amounts)
                        {
                            if (double.IsNaN(amount))
                            {
                                errors.Add($"{filter.PropertyName} için filtrenin boş olmadığından emin olunuz!");
                                break;
                            }
                        }
                    }
                }

                else if (filter.Type == "Date")
                {
                    if (filter.Comparison is null || filter.Dates == null || filter.Dates.Count == 0)
                    {
                        errors.Add($"{filter.PropertyName} için filtrenin boş olmadığından emin olunuz!");
                    }
                    
                }
                else if (filter.Type == "Radio" && filter.CheckedValue is null)
                {
                    errors.Add($"{filter.PropertyName} için filtrenin boş olmadığından emin olunuz!"); 
                }
            }

        return !errors.Any();
    }
}

public class ReadFilterDto
{
    public string Property { get; set; }
    public string PropertyName { get; set; }
    public string Type { get; set; }
    public string? SearchValue { get; set; }
    public List<string>? SelectedValues { get; set; }
    public string? Comparison { get; set; }
    public List<double>? Amounts { get; set; }
    public List<DateTime>? Dates { get; set; }
    public bool? CheckedValue { get; set; }
}