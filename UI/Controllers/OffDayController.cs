﻿
using System.Security.Claims;
using Core;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.OffDayDTOs.WriteDtos;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Abstract.OffDayServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.UserServices;
using Services.ExcelDownloadServices.OffDayServices;
using Services.PdfDownloadServices;


namespace UI.Controllers;
[Authorize]
public class OffDayController : BaseController
{
    private readonly IReadPersonalService _readPersonalService;
    private readonly IWriteOffDayService _writeOffDayService;
    private readonly IReadOffDayService _readOffDayService;
    private readonly OffDayExcelExport _offDayExcelExport;
    private readonly OffDayFormPdf _offDayFormPdf;
    private readonly IReadUserService _readUserService;
    
    public OffDayController(IReadPersonalService readPersonalService, IWriteOffDayService writeOffDayService, IReadOffDayService readOffDayService, OffDayExcelExport offDayExcelExport, OffDayFormPdf offDayFormPdf, IReadUserService readUserService)
    {
        _readPersonalService = readPersonalService;
        _writeOffDayService = writeOffDayService;
        _readOffDayService = readOffDayService;
        _offDayExcelExport = offDayExcelExport;
        _offDayFormPdf = offDayFormPdf;
        _readUserService = readUserService;
    }

	#region PageActions
	/// <summary>
	/// Bekleyen İzinler Listelenme Sayfası
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.Director)},{nameof(UserRoleEnum.SuperAdmin)}")]
	public async Task<IActionResult> WaitingOffDayList(OffdayQuery query)
    {
        var userRole = User.FindFirst(ClaimTypes.Role).Value;
       if (!string.IsNullOrEmpty(userRole) && userRole == nameof(UserRoleEnum.Director))
       {
           var branchesResult = await _readUserService.GetUserBranches(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty));
           if (!branchesResult.IsSuccess) return Redirect("/404");
           query.UserBranches = branchesResult.Data;
           var result = await _readOffDayService.GetSecondWaitingOffDaysListService(query); 
           return View(result);
       }
       else
       {
           var result = await _readOffDayService.GetFirstWaitingOffDaysListService(query);
           return View(result);   
       }
        
    }
	/// <summary>
	/// Bekleyen İzinler Düzenle Sayfası
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.Director)},{nameof(UserRoleEnum.SuperAdmin)}")]
	public async Task<IActionResult> WaitingOffDayEdit(Guid id , string returnUrl)
    {
        var result = await _readOffDayService.GetOffDayEditService(id);
        ViewData["ReturnUrl"] = returnUrl;
        return View(result);
    }
    /// <summary>
    /// İzin Ekleme Formu Sayfası
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.Director)},{nameof(UserRoleEnum.BranchManager)},{nameof(UserRoleEnum.SuperAdmin)}")]
    public async Task<IActionResult> AddOffDay(Guid id)
    {
	    
        var personalResult = await _readPersonalService.GetAllPersonalByBranchIdService(id);
        return View(personalResult);
    }
	/// <summary>
	/// Reddedilen İzinler Listelenme Sayfası
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.Director)},{nameof(UserRoleEnum.SuperAdmin)}")]
	public async Task<IActionResult> RejectedOffDayList(OffdayQuery query)
    {
        var userRole = User.FindFirst(ClaimTypes.Role).Value;
        if (!string.IsNullOrEmpty(userRole) && userRole == nameof(UserRoleEnum.Director))
        {
            var branchesResult = await _readUserService.GetUserBranches(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty));
            if (!branchesResult.IsSuccess) return Redirect("/404");
            query.UserBranches = branchesResult.Data;
        }
        var result = await _readOffDayService.GetRejectedOffDaysListService(query);
        return View(result);
    }
	/// <summary>
	/// Onaylanan İzinler Listelenme Sayfası
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.Director)},{nameof(UserRoleEnum.SuperAdmin)}")]
	public async Task<IActionResult> ApprovedOffDayList(OffdayQuery query)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        if (string.IsNullOrEmpty(userRole) || !GetClientUserId().HasValue) return Redirect("/403");
        if (userRole == nameof(UserRoleEnum.Director))
        {
            var branchesResult = await _readUserService.GetUserBranches(GetClientUserId()!.Value);
            if (!branchesResult.IsSuccess) return Redirect("/404");
            query.UserBranches = branchesResult.Data;
        }
        var result = await _readOffDayService.GetApprovedOffDaysListService(query);
        return View(result);
    }

	#endregion

	#region Get/Post Actions
	/// <summary>
	/// İzin Ekleme Post Metodu
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.Director)},{nameof(UserRoleEnum.BranchManager)},{nameof(UserRoleEnum.SuperAdmin)}")]
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
	        if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
            result = await _writeOffDayService.AddOffDayService(dto,GetClientUserId()!.Value,GetClientIpAddress()); 
        }
        
        return Ok(result);
    }
	/// <summary>
	/// Bekleyen İzinler Düzenleme ve Onaylanan İzin Güncelleme Post Metodu
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.Director)},{nameof(UserRoleEnum.SuperAdmin)}")]
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
	        if(dto.StartDate > dto.EndDate)
		        return Ok(result.SetStatus(false).SetErr("StartDate is bigger than EndDate").SetMessage("İzin başlangıç tarihi bitişten büyük olamaz!")); 
	        if(dto.CountLeave != ((dto.EndDate - dto.StartDate).Days + 1))
		        return Ok(result.SetStatus(false).SetErr("Date and Count not equal").SetMessage("Girdiğiniz Tarih Aralığı İle İzin Günleri Uyuşmuyor."));
	        if(dto.CountLeave <= 0)
		        return Ok(result.SetStatus(false).SetErr("Count Leave is not equal or smaller than zero").SetMessage("Lütfen İzin Girdiğinize Emin Olunuz!"));
	        if(dto.LeaveByYear < 0 || dto.LeaveByTaken < 0 || dto.LeaveByTravel < 0 || dto.LeaveByWeek < 0 || dto.LeaveByFreeDay < 0 || dto.LeaveByPublicHoliday < 0)
		        return Ok(result.SetStatus(false).SetErr("Negative Values").SetMessage("Negatif Değer Girilemez."));
	        if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
            result = dto.OffDayStatus == OffDayStatusEnum.Approved 
	            ? await _writeOffDayService.UpdateApprovedOffDayService(dto,GetClientUserId()!.Value,GetClientIpAddress()) 
	            : await _writeOffDayService.UpdateWaitingOffDayService(dto,GetClientUserId()!.Value,GetClientIpAddress());
        }
        return Ok(result);
    }
    /// <summary>
    /// İlk Bekleyen İzinler Durum Değiştirme(Onayla/Reddet) Post Metodu
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
    [HttpPost]
    public async Task<IActionResult> UpdateFirstWaitingStatus(Guid id , bool status, string returnUrl)
    {
        IResultDto result = new ResultDto();
        var username = User.FindFirst(ClaimTypes.Name).Value;
        if (username.IsNullOrEmpty())
        {
            return Redirect("/404");
        }
        if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
        result = await _writeOffDayService.UpdateFirstWaitingStatusOffDayService(id,status,username,GetClientUserId()!.Value,GetClientIpAddress());
        return Ok(result);
    }
	/// <summary>
	/// İkinci Bekleyen İzinler Durum Değiştirme(Onayla/Reddet) Post Metodu
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.Director)},{nameof(UserRoleEnum.SuperAdmin)}")]
	[HttpPost]
    public async Task<IActionResult> UpdateSecondWaitingStatus(Guid id , bool status, string returnUrl)
    {
        IResultDto result = new ResultDto();
        var username = User.FindFirst(ClaimTypes.Name).Value;
        if (username.IsNullOrEmpty())
        {
            return Redirect("/404");
        }
        if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
        result = await _writeOffDayService.UpdateSecondWaitingStatusOffDayService(id,status,username,GetClientUserId()!.Value,GetClientIpAddress());
        return Ok(result);
    }
	/// <summary>
	/// Onaylanan İzinler İptal Etme Post Metodu
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.Director)},{nameof(UserRoleEnum.SuperAdmin)}")]
	[HttpPost]
    public async Task<IActionResult> DeleteOffDay(Guid id, string returnUrl)
    {
	    if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
        var result = await _writeOffDayService.DeleteOffDayService(id,GetClientUserId()!.Value,GetClientIpAddress());
        return Ok(result);
    }
	/// <summary>
	/// Onaylanan İzinler Excel Raporu Alma
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.Director)},{nameof(UserRoleEnum.SuperAdmin)}")]
	[HttpPost]
    public async Task<IActionResult> ExportExcel(OffdayQuery query , string returnUrl)
    {
        var userRole = User.FindFirst(ClaimTypes.Role).Value;
        if (!string.IsNullOrEmpty(userRole) && userRole == nameof(UserRoleEnum.Director))
        {
            var branchesResult = await _readUserService.GetUserBranches(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty));
            if (!branchesResult.IsSuccess) return Redirect("/404");
            query.UserBranches = branchesResult.Data;
        }
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
            catch
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
	[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.Director)},{nameof(UserRoleEnum.SuperAdmin)}")]
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