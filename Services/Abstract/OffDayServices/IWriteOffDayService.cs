using Core.DTOs.OffDayDTOs;
using Core.Interfaces;

namespace Services.Abstract.OffDayServices;

public interface IWriteOffDayService
{
    Task<bool> ChangeOffDayStatus(Guid id,bool isApproved);
    Task<IResultDto> AddAsync(AddOffdayDto writeDto);
    //Task<IResultWithDataDto<>> UpdateAsync(T writeDto);
    //Task<IResultDto> DeleteAsync(Guid id);
   // Task<IResultDto> RecoverAsync(Guid id);
    //Task<IResultDto> RemoveAsync(Guid id);
}