using Core.Entities;
using Core.Enums;
using Data.Abstract;
using Hangfire;
using Services.Abstract.DailyCounterServices;

namespace Services.Hangfire;

public class DailyJob
{
    private readonly IWriteDailyCounterService _writeDailyCounterService;

    public DailyJob(IWriteDailyCounterService writeDailyCounterService)
    {
        _writeDailyCounterService = writeDailyCounterService;
    }

    public async Task YearLeaveEnhancer()
    {
        var result = await _writeDailyCounterService.AddDailyCounterLogService();
        
    }
    
}