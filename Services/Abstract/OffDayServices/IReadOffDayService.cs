using Core.DTOs.OffDayDTOs;
using Core.Interfaces;

namespace Services.Abstract.OffDayServices;

public interface IReadOffDayService
{
    Task<List<ReadOffDayDto>> GetAllWithPersonal();
    
    
}