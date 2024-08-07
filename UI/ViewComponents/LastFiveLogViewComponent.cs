using Microsoft.AspNetCore.Mvc;
using Services.Abstract.UserLogServices;

namespace UI.ViewComponents;

public class LastFiveLogViewComponent : ViewComponent
{
    private readonly IReadUserLogService _readUserLogService;

    public LastFiveLogViewComponent(IReadUserLogService readUserLogService)
    {
        _readUserLogService = readUserLogService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var result = await _readUserLogService.GetLastFiveLogService();
        return View(result);
    }
}