using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;

namespace UI.Controllers;

public class PersonalDetailController : Controller
{
    private readonly IReadPositionService _readPositionService;
    private readonly IReadBranchService _readBranchService;
    private readonly IReadPersonalService _readPersonalService;
    private readonly IWritePersonalService _writePersonalService;

    public PersonalDetailController(IReadBranchService readBranchService, IReadPositionService readPositionService, IReadPersonalService readPersonalService, IWritePersonalService writePersonalService)
    {
        _readBranchService = readBranchService;
        _readPositionService = readPositionService;
        _readPersonalService = readPersonalService;
        _writePersonalService = writePersonalService;
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _readPersonalService.GetByIdDetailedPersonal(id);
        ViewBag.Positions = await _readPositionService.GetAllJustNames();
        ViewBag.Branches = await _readBranchService.GetAllJustNames();
        return View(result);
    }
    [HttpPost]
    public async Task<IActionResult> Edit()
    {
        
        ViewBag.Positions = await _readPositionService.GetAllJustNames();
        ViewBag.Branches = await _readBranchService.GetAllJustNames();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ArchivePersonal(int id)
    {
        var result = await _writePersonalService.DeleteAsync(id);
        if (result)
        {
            return RedirectToAction("Index", "Personal");
        }

        return BadRequest("Index");
    }
    [HttpPost]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var result = await _writePersonalService.ChangeStatus(id);
        if (result)
        {
            return RedirectToAction("Edit",new { id = id });
        }

        return BadRequest("Index");
    }
}