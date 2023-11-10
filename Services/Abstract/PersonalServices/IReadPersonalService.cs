using Core.DTOs;
using Core.DTOs.PersonalDTOs;
using Core.Interfaces;
using Core.Querys;

namespace Services.Abstract.PersonalServices;

public interface IReadPersonalService : IReadService<PersonalDto>
{
	Task<List<PersonalDto>> GetAllWithBranchAndPositionAsync();
	Task<IResultWithDataDto<List<PersonalDto>>> GetAllWithFilterAsync(PersonalQuery query);
	Task<IResultWithDataDto<PersonalDetailDto>> GetByIdDetailedPersonal(int id);
	Task<ResultWithPagingDataDto<List<PersonalDto>>> GetAllPagingWithBranchAndPositionOrderByAsync(PersonalQuery query);
	Task<ResultWithPagingDataDto<List<PersonalDto>>> GetAllDeletedPersonalPagingOrderByAsync(int pageNumber,string search);
}