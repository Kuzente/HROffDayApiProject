using Core.DTOs.OffDayDTOs;
using Core.DTOs.OffDayDTOs.WriteDtos;
using Core.DTOs.PersonalDTOs;
using Core.Querys;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Services.Abstract.OffDayServices;
using Services.Abstract.PersonalServices;

namespace UI.Controllers;

public class OffDayController : Controller
{
    private readonly IToastNotification _toastNotification;
    private readonly IReadPersonalService _readPersonalService;
    private readonly IWriteOffDayService _writeOffDayService;
    private readonly IReadOffDayService _readOffDayService;

    public OffDayController(IReadPersonalService readPersonalService, IWriteOffDayService writeOffDayService, IToastNotification toastNotification, IReadOffDayService readOffDayService)
    {
        _readPersonalService = readPersonalService;
        _writeOffDayService = writeOffDayService;
        _toastNotification = toastNotification;
        _readOffDayService = readOffDayService;
    }

    #region PageActions
    /// <summary>
    /// Bekleyen İzinler Listelenme Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> WaitingOffDayList(OffdayQuery query)
    {
        var result = await _readOffDayService.GetFirstWaitingOffDaysListService(query);
        if (!result.IsSuccess)
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        return View(result);
    }
    /// <summary>
    /// Bekleyen İzinler Düzenle Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> WaitingOffDayEdit(Guid id , string returnUrl)
    {
        var result = await _readOffDayService.GetFirstWaitingOffDayByIdService(id);
        if (!result.IsSuccess)
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        ViewData["ReturnUrl"] = returnUrl;
        return View(result);
    }
    /// <summary>
    /// İzin Ekleme Formu Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> AddOffDay(Guid id , string returnUrl)
    {
        var personalResult = await _readPersonalService.GetAllPersonalByBranchIdService(id);
        if (personalResult.IsSuccess) return View(personalResult);
        _toastNotification.AddErrorToastMessage(personalResult.Message, new ToastrOptions { Title = "Hata" });
        return Redirect("/subeler" + returnUrl);
    }
    #endregion

    #region Get/Post Actions
    /// <summary>
    /// İzin Ekleme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddOffDay(WriteAddOffDayDto dto)
    {
        if (!ModelState.IsValid)
        {
            _toastNotification.AddErrorToastMessage("Tüm alanları eksiksiz girdiğinize emin olunuz!!", new ToastrOptions { Title = "Hata" });
            return Redirect("/izin-olustur"+ dto.returnUrl);
        }
        var result = await _writeOffDayService.AddOffDayService(dto);
        if (!result.IsSuccess)
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" }); 
        else
            _toastNotification.AddSuccessToastMessage("İzin Talebi Başarılı Bir Şekilde Gönderildi", new ToastrOptions { Title = "Başarılı" }); 
        return Redirect("/izin-olustur"+ dto.returnUrl);
    }
    /// <summary>
    /// Bekleyen İzinler Düzenleme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> WaitingOffDayEdit(WriteUpdateWatingOffDayDto dto)
    {
        if (!ModelState.IsValid)
        {
            _toastNotification.AddErrorToastMessage("Tüm alanları eksiksiz girdiğinize emin olunuz!!", new ToastrOptions { Title = "Hata" });
            return Redirect("/izin-duzenle?id="+dto.ID + "&returnUrl="+ dto.returnUrl);
        }
        var result = await _writeOffDayService.UpdateWaitingOffDayService(dto);
        if (!result.IsSuccess)
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        else
            _toastNotification.AddSuccessToastMessage("İzin Talebi Başarılı Bir Şekilde Güncellendi", new ToastrOptions { Title = "Başarılı" });
        return Redirect("/bekleyen-izinler" + dto.returnUrl);
    }
    /// <summary>
    /// İlk Bekleyen İzinler Durum Değiştirme(Onayla/Reddet) Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> UpdateFirstWaitingStatus(Guid id , bool status, string returnUrl)
    {
        var result = await _writeOffDayService.UpdateFirstWaitingStatusOffDayService(id,status);
        if (!result.IsSuccess)
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        else
            _toastNotification.AddSuccessToastMessage("İşlem Başarılı", new ToastrOptions { Title = "Başarılı" });
        return Redirect("/bekleyen-izinler" + returnUrl);
    }

    #endregion
  
}