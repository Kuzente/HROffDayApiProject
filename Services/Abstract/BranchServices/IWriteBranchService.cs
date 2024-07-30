using Core.DTOs.BranchDTOs;
using Core.DTOs.OffDayDTOs;
using Core.Interfaces;

namespace Services.Abstract.BranchServices;

public interface IWriteBranchService 
{
    Task<IResultDto> AddAsync(BranchDto writeDto,Guid userId,string ipAddress);
    Task<IResultWithDataDto<BranchDto>> UpdateAsync(BranchDto writeDto,Guid userId,string ipAddress);
    Task<IResultDto> DeleteAsync(Guid id,Guid userId,string ipAddress);
    Task<IResultDto> RecoverAsync(Guid id,Guid userId,string ipAddress);
    Task<IResultDto> RemoveAsync(Guid id);
}