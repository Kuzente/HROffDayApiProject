using Core.DTOs.PersonalDTOs.WriteDtos;
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

    #region PageActions
    public async Task<IActionResult> Edit(int id)
    {
        return View();
    }
    

    #endregion

    #region Get/Post Actions
    public async Task<IActionResult> EditAjax(Guid id)
    {
        var result = await _readPersonalService.GetUpdatePersonalService(id);
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(WriteUpdatePersonalDto dto)
    {
        var result = await _writePersonalService.UpdateAsync(dto);
        if (!result.IsSuccess)
        {
            //Error Page Yönlendir TODO
        }

        return RedirectToAction("Index", "Personal");
    }
    [HttpPost]
    public async Task<IActionResult> ArchivePersonal(Guid id)
    {
        var result = await _writePersonalService.DeleteAsync(id);
        if (!result.IsSuccess)
        {
            //Error Page Yönlendir TODO
        }

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> ChangeStatus(Guid id)
    {
        var result = await _writePersonalService.ChangeStatus(id);
        if (!result.IsSuccess)
        {
            //Error Page Yönlendir TODO
        }
        return Ok(result);
    }

    #endregion
  
   
   
}