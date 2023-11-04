using Core.DTOs;
using Core.DTOs.PersonalDTOs;

namespace Services.Abstract.PersonalServices;

public interface IReadPersonalService : IReadService<ReadPersonalDto>
{
	Task<List<ReadPersonalDto>> GetAllWithBranchAndPositionAsync();
	Task<ResultWithPagingDataDto<List<ReadPersonalDto>>> GetAllPagingWithBranchAndPositionOrderByAsync(int pageNumber,string search);
	//Task<ResultWithPagingDataDto<List<ReadPersonalDto>>> GetAllDeletedBranchPagingOrderByAsync(int pageNumber,string search);
}