using Core.DTOs.BranchDTOs;

namespace Services.Abstract.BranchServices;

public interface IReadBranchService : IReadService<ReadBranchDto>
{
	Task<List<ReadBranchDto>> GetAllOrderByAsync();
}