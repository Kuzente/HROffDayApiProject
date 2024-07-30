using Core.Enums;

namespace Core.Entities;

public class UserLog : BaseEntity
{
    public string EntityName { get; set; }
    public string Description { get; set; }
    public string IpAddress { get; set; }
    public LogType LogType { get; set; }
    public User User { get; set; }
    public Guid UserID { get; set; }
    
}