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
    public async Task<IActionResult> Index()
    {
        var result = await _readDailyCounterService.GetLastHundredLogService();
        
        return View(result);
    }
}