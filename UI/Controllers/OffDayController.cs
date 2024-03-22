
using System.Security.Claims;
using Core;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.OffDayDTOs.WriteDtos;
using Core.Interfaces;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.OffDayServices;
using Services.Abstract.PersonalServices;
using Services.ExcelDownloadServices.OffDayServices;
using Services.PdfDownloadServices;


namespace UI.Controllers;
[Authorize]
public class OffDayController : Controller
{
    private readonly IReadPersonalService _readPersonalService;
    private readonly IWriteOffDayService _writeOffDayService;
    private readonly IReadOffDayService _readOffDayService;
    private readonly OffDayExcelExport _offDayExcelExport;
    private readonly OffDayFormPdf _offDayFormPdf;
    
    public OffDayController(IReadPersonalService readPersonalService, IWriteOffDayService writeOffDayService, IReadOffDayService readOffDayService, OffDayExcelExport offDayExcelExport, OffDayFormPdf offDayFormPdf)
    {
        _readPersonalService = readPersonalService;
        _writeOffDayService = writeOffDayService;
        _readOffDayService = readOffDayService;
        _offDayExcelExport = offDayExcelExport;
        _offDayFormPdf = offDayFormPdf;
    }

    #region PageActions
    /// <summary>
    /// Bekleyen İzinler Listelenme Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> WaitingOffDayList(OffdayQuery query)
    {
        var userCookie = User.FindFirst(ClaimTypes.Name).Value;
        if (!string.IsNullOrEmpty(userCookie) && userCookie == "samicangulcan")
        {
            var result = await _readOffDayService.GetFirstWaitingOffDaysListService(query);
            return View(result);
        }
        //İnsan Kaynakları service
       else if (!string.IsNullOrEmpty(userCookie) && userCookie == "azimyilmaz")
       {
           var result = await _readOffDayService.GetSecondWaitingOffDaysListService(query); 
           return View(result);
       }
       else
       {
           query.branchName = "Iyaş Park";
           var result = await _readOffDayService.GetSecondWaitingOffDaysListService(query); 
           return View(result);   
       }
        
    }
    /// <summary>
    /// Bekleyen İzinler Düzenle Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> WaitingOffDayEdit(Guid id , string returnUrl)
    {
        var result = await _readOffDayService.GetOffDayByIdService(id);
        ViewData["ReturnUrl"] = returnUrl;
        return View(result);
    }
    /// <summary>
    /// İzin Ekleme Formu Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> AddOffDay(Guid id)
    {
        var personalResult = await _readPersonalService.GetAllPersonalByBranchIdService(id);
        return View(personalResult);
    }
    /// <summary>
    /// Reddedilen İzinler Listelenme Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> RejectedOffDayList(OffdayQuery query)
    {
        var result = await _readOffDayService.GetRejectedOffDaysListService(query);
        return View(result);
    }
    /// <summary>
    /// Onaylanan İzinler Listelenme Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> ApprovedOffDayList(OffdayQuery query)
    {
        var result = await _readOffDayService.GetApprovedOffDaysListService(query);
        return View(result);
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
        IResultDto result = new ResultDto();
        if (!ModelState.IsValid)
        {
            result.SetStatus(false).SetErr("Modelstate is not valid").SetMessage("Lütfen Zorunlu Alanların Girildiğinden Emin Olunuz.");
        }
        else
        {
            result = await _writeOffDayService.AddOffDayService(dto); 
        }
        
        return Ok(result);
    }
    /// <summary>
    /// Bekleyen İzinler Düzenleme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> WaitingOffDayEdit(WriteUpdateWatingOffDayDto dto)
    {
        IResultDto result = new ResultDto();
        if (!ModelState.IsValid)
        {
            result.SetStatus(false).SetErr("Modelstate is not valid").SetMessage("Lütfen Zorunlu Alanların Girildiğinden Emin Olunuz.");
        }
        else
        {
            result = await _writeOffDayService.UpdateWaitingOffDayService(dto);
        }
        return Ok(result);
    }
    /// <summary>
    /// İlk Bekleyen İzinler Durum Değiştirme(Onayla/Reddet) Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> UpdateFirstWaitingStatus(Guid id , bool status, string returnUrl)
    {
        var result = await _writeOffDayService.UpdateFirstWaitingStatusOffDayService(id,status);
        return Ok(result);
    }
    /// <summary>
    /// İkinci Bekleyen İzinler Durum Değiştirme(Onayla/Reddet) Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> UpdateSecondWaitingStatus(Guid id , bool status, string returnUrl)
    {
        var result = await _writeOffDayService.UpdateSecondWaitingStatusOffDayService(id,status);
        return Ok(result);
    }
    /// <summary>
    /// Onaylanan İzinler İptal Etme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> DeleteOffDay(Guid id, string returnUrl)
    {
        var result = await _writeOffDayService.DeleteOffDayService(id);
        return Ok(result);
    }
    /// <summary>
    /// Onaylanan İzinler Excel Raporu Alma
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ExportExcel(OffdayQuery query , string returnUrl)
    {
        var result = await _readOffDayService.GetExcelApprovedOffDayListService(query);
        if (result.IsSuccess)
        {
            try
            {
                byte[] excelData = _offDayExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.
                var response = HttpContext.Response;
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.Headers.Add("Content-Disposition", "attachment; filename=Izinler.xlsx");
                await response.Body.WriteAsync(excelData, 0, excelData.Length);
                return new EmptyResult();
            }
            catch (Exception e)
            {
                return Redirect(returnUrl);
            }
        }
        return Redirect(returnUrl);
    }
    /// <summary>
    /// Onaylanan İzinler Pdf Formu Alma
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ExportPdf(Guid id,string returnUrl)
    {
        var result = await _readOffDayService.GetApprovedOffDayExcelFormService(id);
        if (result.IsSuccess)
        {
            byte[] pdfFile = _offDayFormPdf.GetOffDayPdfDocument(result.Data);
            var response = HttpContext.Response;
            response.ContentType = "application/pdf";
            response.Headers.Add("Content-Disposition", "attachment; filename=IzinFormu.pdf");
            await response.Body.WriteAsync(pdfFile, 0, pdfFile.Length);
            return new EmptyResult();
        }
        return Redirect(returnUrl);
    }
    #endregion
  
}