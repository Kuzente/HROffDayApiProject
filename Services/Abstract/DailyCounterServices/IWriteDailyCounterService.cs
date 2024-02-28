using Core.Interfaces;

namespace Services.Abstract.DailyCounterServices;

public interface IWriteDailyCounterService
{
    Task<IResultDto> AddDailyCounterLogService();
}