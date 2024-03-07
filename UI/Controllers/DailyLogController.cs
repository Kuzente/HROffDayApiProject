using Microsoft.AspNetCore.Mvc;
using Services.Abstract.DailyCounterServices;

namespace UI.Controllers;

public class DailyLogController : Controller
{
    private readonly IReadDailyCounterService _readDailyCounterService;

    public DailyLogController(IReadDailyCounterService readDailyCounterService)
    {
        _readDailyCounterService = readDailyCounterService;
    }

    // GET
    public IActionResult Index()
    {
        //var result = await _readDailyCounterService.GetLastHundredDailyYearLogService();
        
        return View();
    }
    // GET
    public async Task<IActionResult> GetYearLogs()
    {
        var result = await _readDailyCounterService.GetLastHundredDailyYearLogService();
        return Ok(result);
    }
    public async Task<IActionResult> GetFoodLogs()
    {
        var result = await _readDailyCounterService.GetLastHundredDailyFoodLogService();
        return Ok(result);
    }
}