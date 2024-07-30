using Core.DTOs.BranchDTOs;
using Core.DTOs.OffDayDTOs;
using Core.DTOs.PositionDTOs;
using Core.Interfaces;

namespace Services.Abstract.PositionServices;

public interface IWritePositionService
{
    Task<IResultDto> AddAsync(PositionDto writeDto,Guid userId,string ipAddress);
    Task<IResultWithDataDto<PositionDto>> UpdateAsync(PositionDto writeDto,Guid userId,string ipAddress);
    Task<IResultDto> DeleteAsync(Guid id,Guid userId,string ipAddress);
    Task<IResultDto> RecoverAsync(Guid id,Guid userId,string ipAddress);
    
}