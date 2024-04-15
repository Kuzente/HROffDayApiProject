using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.DailyCounterServices;

namespace UI.Controllers;
[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.Director)},{nameof(UserRoleEnum.SuperAdmin)}")]
public class DailyLogController : Controller
{
    private readonly IReadDailyCounterService _readDailyCounterService;

    public DailyLogController(IReadDailyCounterService readDailyCounterService)
    {
        _readDailyCounterService = readDailyCounterService;
    }

    #region PageActions

    // GET
    public IActionResult Index()
    {
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

    #endregion
}