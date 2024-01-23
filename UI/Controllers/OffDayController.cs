
using Core.DTOs.OffDayDTOs.WriteDtos;

using Core.Querys;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Services.Abstract.OffDayServices;
using Services.Abstract.PersonalServices;
using Services.ExcelDownloadServices;
using Services.ExcelDownloadServices.OffDayServices;

namespace UI.Controllers;

public class OffDayController : Controller
{
    private readonly IToastNotification _toastNotification;
    private readonly IReadPersonalService _readPersonalService;
    private readonly IWriteOffDayService _writeOffDayService;
    private readonly IReadOffDayService _readOffDayService;
    private readonly OffDayExcelExport _offDayExcelExport;
    private readonly ApprovedOffdayFormExcelExport _formExcelExport;

    public OffDayController(IReadPersonalService readPersonalService, IWriteOffDayService writeOffDayService, IToastNotification toastNotification, IReadOffDayService readOffDayService, OffDayExcelExport offDayExcelExport, ApprovedOffdayFormExcelExport formExcelExport)
    {
        _readPersonalService = readPersonalService;
        _writeOffDayService = writeOffDayService;
        _toastNotification = toastNotification;
        _readOffDayService = readOffDayService;
        _offDayExcelExport = offDayExcelExport;
        _formExcelExport = formExcelExport;
    }

    #region PageActions
    /// <summary>
    /// Bekleyen İzinler Listelenme Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> WaitingOffDayList(OffdayQuery query)
    {
        //İnsan Kaynakları service
       //var result = await _readOffDayService.GetFirstWaitingOffDaysListService(query);
        
        var result = await _readOffDayService.GetSecondWaitingOffDaysListService(query);
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
        return Redirect(returnUrl);
    }
    /// <summary>
    /// Reddedilen İzinler Listelenme Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> RejectedOffDayList(OffdayQuery query)
    {
        var result = await _readOffDayService.GetRejectedOffDaysListService(query);
        if (!result.IsSuccess)
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        return View(result);
    }
    /// <summary>
    /// Onaylanan İzinler Listelenme Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> ApprovedOffDayList(OffdayQuery query)
    {
        var result = await _readOffDayService.GetApprovedOffDaysListService(query);
        if (!result.IsSuccess)
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        return View(result);
    }
    [HttpPost]
    public async Task<IActionResult> createpdf(Guid id,string returnUrl)
    {
        var result = await _readOffDayService.GetApprovedOffDayExcelFormService(id);
        if(!result.IsSuccess)
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        else
        {
            var excelFormPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "offdayForm.xlsx");
            byte[] excelFile = _formExcelExport.ExportToExcel(result.Data , excelFormPath);
            var response = HttpContext.Response;
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.Headers.Add("Content-Disposition", "attachment; filename=IzinFormu.xlsx");
            await response.Body.WriteAsync(excelFile, 0, excelFile.Length);
            return new EmptyResult();
        }
        return Redirect(returnUrl);
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
        return Redirect(dto.returnUrl);
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
        return Redirect(dto.returnUrl);
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
        return Redirect(returnUrl);
    }
    /// <summary>
    /// İkinci Bekleyen İzinler Durum Değiştirme(Onayla/Reddet) Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> UpdateSecondWaitingStatus(Guid id , bool status, string returnUrl)
    {
        var result = await _writeOffDayService.UpdateSecondWaitingStatusOffDayService(id,status);
        if (!result.IsSuccess)
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        else
            _toastNotification.AddSuccessToastMessage("İşlem Başarılı", new ToastrOptions { Title = "Başarılı" });
        return Redirect(returnUrl);
    }
    /// <summary>
    /// Onaylanan İzinler İptal Etme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> DeleteOffDay(Guid id, string returnUrl)
    {
        var result = await _writeOffDayService.DeleteOffDayService(id);
        if (!result.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        }
        else
        {
            _toastNotification.AddSuccessToastMessage("İzin Başarılı Bir Şekilde Silindi", new ToastrOptions { Title = "Başarılı" }); 
        }
        return Redirect(returnUrl);
    }
    /// <summary>
    /// Bekleyen İzinler Düzenleme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ExportExcel(OffdayQuery query , string returnUrl)
    {
        var result = await _readOffDayService.GetExcelOffDayListService(query);
        if (result.IsSuccess)
        {
            byte[] excelData = _offDayExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.
            var response = HttpContext.Response;
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.Headers.Add("Content-Disposition", "attachment; filename=Izinler.xlsx");
            await response.Body.WriteAsync(excelData, 0, excelData.Length);
            return new EmptyResult();
        }

        _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        return Redirect(returnUrl);
    }
    #endregion
  
}