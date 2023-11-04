using Core.DTOs.BranchDTOs;
using Core.DTOs;
using Core.DTOs.PositionDTOs;
using Core.Interfaces;

namespace Services.Abstract.PositionServices;

public interface IReadPositionService : IReadService<PositionDto>
{
	Task<bool> GetAnyByNameAsync(string name);
	Task<List<PositionNameDto>> GetAllJustNames();
    Task<ResultWithPagingDataDto<List<PositionDto>>> GetAllPagingOrderByAsync(int pageNumber, string search);
    Task<ResultWithPagingDataDto<List<PositionDto>>> GetAllDeletedBranchPagingOrderByAsync(int pageNumber,string search);
    Task<IResultWithDataDto<PositionDto>> GetByIdUpdate(int id);
}