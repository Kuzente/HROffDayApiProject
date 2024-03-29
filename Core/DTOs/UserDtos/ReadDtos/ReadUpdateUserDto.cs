using Core.DTOs.BranchDTOs;
using Core.Enums;

namespace Core.DTOs.UserDtos.ReadDtos;

public class ReadUpdateUserDto
{
    public Guid ID { get; set; }
    public string Username { get; set; }
    public UserRoleEnum Role { get; set; }
    public List<BranchNameDto> SelectedBranches { get; set; }
    public List<BranchNameDto> Branches { get; set; }
    public EntityStatusEnum Status { get; set; }
    public DateTime CreatedAt { get; set; }
}