namespace UI.Models;

public class DatatableReturn<T>
{
    public int RecordsTotal { get; set; }
    public int RecordsFiltered { get; set; }
    public List<T>? Data { get; set; }
    public string Err { get; set; }
    public string Message { get; set; }
    public bool issuccess { get; set; }
}