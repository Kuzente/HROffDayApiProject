using Core.DTOs.MissingDayDtos.WriteDtos;
using Core.Interfaces;

namespace Services.Abstract.MissingDayServices;

public interface IWriteMissingDayService
{
   Task<IResultDto> AddMissingDayService(WriteAddMissingDayDto dto);
   Task<IResultDto> DeleteMissingDayService(Guid id);
}