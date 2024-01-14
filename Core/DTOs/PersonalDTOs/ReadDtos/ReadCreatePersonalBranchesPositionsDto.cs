using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;

namespace Core.DTOs.PersonalDTOs.ReadDtos;

public class ReadCreatePersonalBranchesPositionsDto
{
    public List<BranchNameDto> Branches { get; set; }
    public List<PositionNameDto> Positions { get; set; }
}