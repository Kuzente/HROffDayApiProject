using Core.DTOs.BranchDTOs;
using Core.DTOs.OffDayDTOs;
using Core.DTOs.PositionDTOs;
using Core.Interfaces;

namespace Services.Abstract.PositionServices;

public interface IWritePositionService
{
    Task<IResultDto> AddAsync(PositionDto writeDto);
    Task<IResultWithDataDto<PositionDto>> UpdateAsync(PositionDto writeDto);
    Task<IResultDto> DeleteAsync(Guid id);
    Task<IResultDto> RecoverAsync(Guid id);
    Task<IResultDto> RemoveAsync(Guid id);
}