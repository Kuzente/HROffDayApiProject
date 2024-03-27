using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.DTOs.UserDtos.ReadDtos;

public class ReadUserSignInDto
{
    public Guid ID { get; set; }
    public string Username { get; set; } = string.Empty;
    public UserRoleEnum Role { get; set; }
    [Required,StringLength(150)]
    public string Email { get; set; }
    [Required,StringLength(maximumLength:20,MinimumLength =6)]
    public string Password { get; set; }
    [Required]
    public string Security { get; set; }
}
