namespace UI.Models;

public class LoginRequests
{
    public int RequestNumber { get; set; }
    public bool IsRestricted { get; set; }
    public DateTime LastRequestDate { get; set; }
}