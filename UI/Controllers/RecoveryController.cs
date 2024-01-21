using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Services.Abstract.BranchServices;
using Services.Abstract.OffDayServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;

namespace UI.Controllers;

public class RecoveryController : Controller
{
    private readonly IToastNotification _toastNotification;
    private readonly IReadBranchService _readBranchService;
    private readonly IWriteBranchService _writeBranchService;
    private readonly IReadPositionService _readPositionService;
    private readonly IWritePositionService _writePositionService;
    private readonly IReadPersonalService _readPersonalService;
    private readonly IWritePersonalService _writePersonalService;
    private readonly IReadOffDayService _readOffDayService;

    public RecoveryController(IReadBranchService readBranchService, IReadPositionService readPositionService, IReadPersonalService readPersonalService, IWriteBranchService writeBranchService, IWritePositionService writePositionService, IWritePersonalService writePersonalService, IToastNotification toastNotification, IReadOffDayService readOffDayService)
    {
        _readBranchService = readBranchService;
        _readPositionService = readPositionService;
        _readPersonalService = readPersonalService;
        _writeBranchService = writeBranchService;
        _writePositionService = writePositionService;
        _writePersonalService = writePersonalService;
        _toastNotification = toastNotification;
        _readOffDayService = readOffDayService;
    }

    #region PageActions
    /// <summary>
    /// Silinen Şubeler Listesi Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> DeletedBranch(string search, int sayfa = 1)
    {
        var result = await _readBranchService.GetDeletedBranchListService(sayfa, search);
        if (!result.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        }
        return View(result);
    }
    /// <summary>
    /// Silinen Ünvanlar Listesi Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> DeletedPosition(string search, int sayfa = 1)
    {
        var result = await _readPositionService.GetDeletedPositionListService(sayfa, search);
        if (!result.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        }
        return View(result);
    }
    /// <summary>
    /// Silinen Personeller Listesi Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> DeletedPersonal(string search, int sayfa = 1)
    {
        var result = await _readPersonalService.GetDeletedPersonalListService(sayfa, search);
        if (!result.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        }
        return View(result);
    }
    /// <summary>
    /// Silinen İzinler Listesi Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> DeletedOffDay(string search, int sayfa = 1)
    {
        var result = await _readOffDayService.GetDeletedOffDaysListService(sayfa, search);
        if (!result.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        }
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
        var result = await _writeBranchService.RecoverAsync(id);
        if (!result.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        }
        else
        {
            _toastNotification.AddSuccessToastMessage("Şube Başarılı Bir Şekilde Geri Döndürüldü.", new ToastrOptions { Title = "Başarılı" }); 
        }

        return Redirect("/silinen-subeler" + returnUrl);
    }
    /// <summary>
    /// Silinen Ünvan Geri Döndür Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RecoverPosition(Guid id, string returnUrl)
    {
        var result = await _writePositionService.RecoverAsync(id);
        if (!result.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        }
        else
        {
            _toastNotification.AddSuccessToastMessage("Ünvan Başarılı Bir Şekilde Geri Döndürüldü.", new ToastrOptions { Title = "Başarılı" }); 
        }
        return Redirect("/silinen-unvanlar" + returnUrl);
    }
    /// <summary>
    /// Silinen Personel Geri Döndür Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RecoverPersonal(Guid id, string returnUrl)
    {
        var result = await _writePersonalService.RecoverAsync(id);
        if (!result.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        }
        else
        {
            _toastNotification.AddSuccessToastMessage("Personel Başarılı Bir Şekilde Geri Döndürüldü.", new ToastrOptions { Title = "Başarılı" }); 
        }
        return Redirect("/silinen-personeller" + returnUrl);
    }

    #endregion
}