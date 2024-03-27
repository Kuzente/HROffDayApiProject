using Core.DTOs.BranchDTOs;
using Core.Enums;

namespace Core.DTOs.UserDtos.ReadDtos;

public class UserListDto
{
    public Guid ID { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsDefaultPassword { get; set; }
    public UserRoleEnum Role { get; set; }
    public List<BranchNameDto> Branches { get; set; }
    public EntityStatusEnum Status { get; set; }
}