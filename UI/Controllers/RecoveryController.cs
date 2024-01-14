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
    public async Task<IActionResult> DeletedBranch(string search, int sayfa = 1)
    {
        var result = await _readBranchService.GetAllDeletedBranchPagingOrderByAsync(sayfa, search);
        return View(result);
    }
    [HttpGet]
    public async Task<IActionResult> DeletedPosition(string search, int sayfa = 1)
    {
        var result = await _readPositionService.GetAllDeletedPositionPagingOrderByAsync(sayfa, search);
        return View(result);
    }
    [HttpGet]
    public async Task<IActionResult> DeletedPersonal(string search, int sayfa = 1)
    {
        var result = await _readPersonalService.GetAllDeletedPersonalPagingOrderByAsync(sayfa, search);
        return View(result);
    }
    [HttpGet]
    public async Task<IActionResult> RecoverBranch(Guid id,  string returnUrl) 
    {
        var result = await _writeBranchService.RecoverAsync(id);
        if (!result.IsSuccess)
        {
            // Error Page Yönlendir TODO
        }

        return Redirect("/silinen-subeler" + returnUrl);
    }
    [HttpGet]
    public async Task<IActionResult> RecoverPosition(Guid id, string returnUrl)
    {
        var result = await _writePositionService.RecoverAsync(id);
        if (!result.IsSuccess)
        {
            // Error Page Yönlendir TODO
        }
        return Redirect("/silinen-unvanlar" + returnUrl);
    }
    [HttpGet]
    public async Task<IActionResult> RecoverPersonal(Guid id, string returnUrl)
    {
        var result = await _writePersonalService.RecoverAsync(id);
        if (!result.IsSuccess)
        {
            // Error Page Yönlendir TODO
        }
        return Redirect("/silinen-personeller" + returnUrl);
    }
}