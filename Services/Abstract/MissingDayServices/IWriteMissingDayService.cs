using Core.DTOs.MissingDayDtos.WriteDtos;
using Core.Interfaces;

namespace Services.Abstract.MissingDayServices;

public interface IWriteMissingDayService
{
   Task<IResultDto> AddMissingDayService(WriteAddMissingDayDto dto,Guid userId,string ipAddress);
   Task<IResultDto> DeleteMissingDayService(Guid id,Guid userId,string ipAddress);
}