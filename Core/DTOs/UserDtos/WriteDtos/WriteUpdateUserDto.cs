using Core.Enums;

namespace Core.DTOs.UserDtos.WriteDtos;

public class WriteUpdateUserDto
{
    public Guid ID { get; set; }
    public string Username { get; set; }
    public EntityStatusEnum Status { get; set; }
    public UserRoleEnum Role { get; set; }
    public List<Guid>? BranchNames { get; set; }
    public DateTime CreatedAt { get; set; }
}