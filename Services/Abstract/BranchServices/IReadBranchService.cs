using Core.DTOs;
using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.BranchDTOs.ReadDtos;
using Core.Interfaces;

namespace Services.Abstract.BranchServices;

public interface IReadBranchService
{
	Task<IResultWithDataDto<List<ReadBranchListDto>>> GetBranchListService(FilterDto filter);
	Task<IResultWithDataDto<List<BranchDto>>> GetAllOrderByAsync();
	Task<ResultWithPagingDataDto<List<BranchDto>>> GetAllPagingOrderByAsync(int pageNumber,string search, bool passive);
	Task<ResultWithPagingDataDto<List<BranchDto>>> GetAllDeletedBranchPagingOrderByAsync(int pageNumber,string search);
	Task<IResultWithDataDto<BranchDto>> GetByIdUpdate(Guid id);
    Task<List<BranchNameDto>> GetAllJustNames();
    Task<IQueryable> GetBranchesOdataService();


}