using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Interfaces;

namespace Services.Abstract.BranchServices;

public interface IReadBranchService : IReadService<ReadBranchDto>
{
	//Task<List<ReadBranchDto>> GetAllOrderByAsync();
	Task<IResultWithDataDto<List<ReadBranchDto>>> GetAllOrderByAsync();
	Task<ResultWithPagingDataDto<List<ReadBranchDto>>> GetAllPagingOrderByAsync(int pageNumber);
	
	
}