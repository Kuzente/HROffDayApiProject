using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;

namespace UI.Controllers;

public class RecoveryController : Controller
{
    private readonly IReadBranchService _readBranchService;
    private readonly IWriteBranchService _writeBranchService;
    private readonly IReadPositionService _readPositionService;
    private readonly IWritePositionService _writePositionService;
    private readonly IReadPersonalService _readPersonalService;
    private readonly IWritePersonalService _writePersonalService;

    public RecoveryController(IReadBranchService readBranchService, IReadPositionService readPositionService, IReadPersonalService readPersonalService, IWriteBranchService writeBranchService, IWritePositionService writePositionService, IWritePersonalService writePersonalService)
    {
        _readBranchService = readBranchService;
        _readPositionService = readPositionService;
        _readPersonalService = readPersonalService;
        _writeBranchService = writeBranchService;
        _writePositionService = writePositionService;
        _writePersonalService = writePersonalService;
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
    [HttpGet]
    public async Task<IActionResult> RecoverBranch(int id, int pageNumber = 1)
    {
        var result = await _writeBranchService.RecoverAsync(id);
        if (!result.IsSuccess)
        {
            // Error Page Yönlendir TODO
        }
        return RedirectToAction("DeletedBranch",new { pageNumber = pageNumber });
    }
    [HttpGet]
    public async Task<IActionResult> RecoverPosition(int id, int pageNumber = 1)
    {
        var result = await _writePositionService.RecoverAsync(id);
        if (!result.IsSuccess)
        {
            // Error Page Yönlendir TODO
        }
        return RedirectToAction("DeletedPosition",new { pageNumber = pageNumber });
    }
    [HttpGet]
    public async Task<IActionResult> RecoverPersonal(int id, int pageNumber = 1)
    {
        var result = await _writePersonalService.RecoverAsync(id);
        if (!result.IsSuccess)
        {
            // Error Page Yönlendir TODO
        }
        return RedirectToAction("DeletedPersonal",new { pageNumber = pageNumber });
    }
}