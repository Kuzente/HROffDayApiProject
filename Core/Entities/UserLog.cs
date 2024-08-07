using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class UserLog : BaseEntity
{
    [Required]
    public string EntityName { get; set; }
	[Required]
	public string Description { get; set; }
	[Required]
	public string IpAddress { get; set; }
	[Required]
	public LogType LogType { get; set; }
	[ForeignKey(nameof(UserID))]
	public User User { get; set; }
	[Required]
    public Guid UserID { get; set; }
}