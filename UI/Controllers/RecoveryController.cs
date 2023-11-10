using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;

namespace UI.Controllers;

public class RecoveryController : Controller
{
    private readonly IReadBranchService _readBranchService;
    private readonly IReadPositionService _readPositionService;
    private readonly IReadPersonalService _readPersonalService;

    public RecoveryController(IReadBranchService readBranchService, IReadPositionService readPositionService, IReadPersonalService readPersonalService)
    {
        _readBranchService = readBranchService;
        _readPositionService = readPositionService;
        _readPersonalService = readPersonalService;
    }
    [HttpGet]
    public async Task<IActionResult> DeletedBranch(string search, int pageNumber = 1)
    {
        var result = await _readBranchService.GetAllDeletedBranchPagingOrderByAsync(pageNumber, search);
        return View(result);
    }
    [HttpGet]
    public async Task<IActionResult> DeletedPosition(string search, int pageNumber = 1)
    {
        var result = await _readPositionService.GetAllDeletedPositionPagingOrderByAsync(pageNumber, search);
        return View(result);
    }
    [HttpGet]
    public async Task<IActionResult> DeletedPersonal(string search, int pageNumber = 1)
    {
        var result = await _readPersonalService.GetAllDeletedPersonalPagingOrderByAsync(pageNumber, search);
        return View(result);
    }
}