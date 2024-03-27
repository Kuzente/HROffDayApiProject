using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Entities;

public partial class User : BaseEntity
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public bool IsDefaultPassword { get; set; }
    [Required] 
    public UserRoleEnum Role { get; set; }
}
public partial class User
{
    public ICollection<BranchUser> BranchUsers { get; set; }
}