using Core.DTOs.BranchDTOs;
using Core.DTOs.OffDayDTOs;
using Core.Interfaces;

namespace Services.Abstract.BranchServices;

public interface IWriteBranchService 
{
    Task<IResultDto> AddAsync(BranchDto writeDto);
    Task<IResultWithDataDto<BranchDto>> UpdateAsync(BranchDto writeDto);
    Task<IResultDto> DeleteAsync(Guid id);
    Task<IResultDto> RecoverAsync(Guid id);
    Task<IResultDto> RemoveAsync(Guid id);
}