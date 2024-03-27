using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.DTOs.UserDtos.WriteDtos;

public class AddUserDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required] 
    public UserRoleEnum Role { get; set; }
    
    public List<Guid>? BranchNames { get; set; }
}

