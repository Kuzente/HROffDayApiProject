using System.Security.Claims;
using Core.Enums;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.OffDayServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;

namespace UI.Controllers;
[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
public class RecoveryController : BaseController
{
    private readonly IReadBranchService _readBranchService;
    private readonly IWriteBranchService _writeBranchService;
    private readonly IReadPositionService _readPositionService;
    private readonly IWritePositionService _writePositionService;
    private readonly IReadPersonalService _readPersonalService;
    private readonly IWritePersonalService _writePersonalService;
    private readonly IReadOffDayService _readOffDayService;

    public RecoveryController(IReadBranchService readBranchService, IReadPositionService readPositionService, IReadPersonalService readPersonalService, IWriteBranchService writeBranchService, IWritePositionService writePositionService, IWritePersonalService writePersonalService, IReadOffDayService readOffDayService)
    {
        _readBranchService = readBranchService;
        _readPositionService = readPositionService;
        _readPersonalService = readPersonalService;
        _writeBranchService = writeBranchService;
        _writePositionService = writePositionService;
        _writePersonalService = writePersonalService;
        _readOffDayService = readOffDayService;
    }

    #region PageActions
    /// <summary>
    /// Silinen Şubeler Listesi Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> DeletedBranch(BranchQuery query)
    {
        var result = await _readBranchService.GetDeletedBranchListService(query);
        return View(result);
    }
    /// <summary>
    /// Silinen Ünvanlar Listesi Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> DeletedPosition(PositionQuery query)
    {
        var result = await _readPositionService.GetDeletedPositionListService(query);
        return View(result);
    }
    /// <summary>
    /// Silinen Personeller Listesi Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> DeletedPersonal(PersonalQuery query)
    {
        var result = await _readPersonalService.GetDeletedPersonalListService(query);
        return View(result);
    }
    /// <summary>
    /// Silinen İzinler Listesi Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> DeletedOffDay(OffdayQuery query)
    {
        var result = await _readOffDayService.GetDeletedOffDaysListService(query);
        return View(result);
    }

    #endregion

    #region Get/Post Actions

    /// <summary>
    /// Silinen Şube Geri Döndür Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RecoverBranch(Guid id,  string returnUrl) 
    {
        if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
        var result = await _writeBranchService.RecoverAsync(id,GetClientUserId().Value,GetClientIpAddress());
        return Ok(result);
    }
    /// <summary>
    /// Silinen Ünvan Geri Döndür Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RecoverPosition(Guid id, string returnUrl)
    {
        if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
        var result = await _writePositionService.RecoverAsync(id,GetClientUserId().Value,GetClientIpAddress());
        return Ok(result);
    }
    /// <summary>
    /// Silinen Personel Geri Döndür Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RecoverPersonal(Guid id, string returnUrl)
    {
        if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
        var result = await _writePersonalService.RecoverAsync(id,GetClientUserId().Value,GetClientIpAddress());
        
        return Ok(result);
    }

    #endregion
}