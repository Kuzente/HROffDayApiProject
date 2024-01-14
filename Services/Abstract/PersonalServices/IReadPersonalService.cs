using Core.DTOs;
using Core.DTOs.PassivePersonalDtos;
using Core.DTOs.PersonalDetailDto.ReadDtos;
using Core.DTOs.PersonalDTOs;
using Core.DTOs.PersonalDTOs.ReadDtos;
using Core.Interfaces;
using Core.Querys;

namespace Services.Abstract.PersonalServices;

public interface IReadPersonalService
{
	Task<IResultWithDataDto<List<PersonalDto>>> GetAllWithFilterAsync(PersonalQuery query);
	Task<IResultWithDataDto<List<PassivePersonalDto>>> PassiveGetAllWithFilterAsync(PersonalQuery query);
	Task<IResultWithDataDto<ReadUpdatePersonalDto>> GetUpdatePersonal(Guid id);
	Task<ResultWithPagingDataDto<List<PersonalDto>>> GetAllPagingWithBranchAndPositionOrderByAsync(PersonalQuery query);
	Task<ResultWithPagingDataDto<List<PassivePersonalDto>>> PassivePersonalGetAllPagingWithBranchAndPositionOrderByAsync(PersonalQuery query);
	Task<ResultWithPagingDataDto<List<PersonalDto>>> GetAllDeletedPersonalPagingOrderByAsync(int pageNumber,string search);
	Task<IResultWithDataDto<List<PersonalOffDayDto>>> GetAllPersonalByBranchId(Guid branchId);
}