using Core;
using Core.DTOs.MissingDayDtos.WriteDtos;
using Core.DTOs.PersonalCumulativeDtos.WriteDtos;
using Core.DTOs.PersonalDetailDto.WriteDtos;
using Core.DTOs.PersonalDTOs.WriteDtos;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.MissingDayServices;
using Services.Abstract.OffDayServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;
using Services.Abstract.TransferPersonalService;
using Services.ExcelDownloadServices.MissingDayServices;
using Services.ExcelDownloadServices.TransferPersonalServices;

namespace UI.Controllers;

[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
public class PersonalDetailController : Controller
{
    private readonly IReadPersonalService _readPersonalService;
    private readonly IReadOffDayService _readOffDayService;
    private readonly IWritePersonalService _writePersonalService;
    private readonly IReadTransferPersonalService _readTransferPersonalService;
    private readonly IWriteTransferPersonalService _writeTransferPersonalService;
    private readonly TransferPersonalExcelExport _transferPersonalExcelExport;
    private readonly IReadMissingDayService _readMissingDayService;
    private readonly IWriteMissingDayService _writeMissingDayService;
    private readonly MissingDayPersonalExcelExport _missingDayPersonalExcelExport;

    public PersonalDetailController(IReadPersonalService readPersonalService, IWritePersonalService writePersonalService,IReadOffDayService readOffDayService, IReadTransferPersonalService readTransferPersonalService, IWriteTransferPersonalService writeTransferPersonalService, TransferPersonalExcelExport transferPersonalExcelExport, IReadMissingDayService readMissingDayService, IWriteMissingDayService writeMissingDayService, MissingDayPersonalExcelExport missingDayPersonalExcelExport)
    {
        _readPersonalService = readPersonalService;
        _writePersonalService = writePersonalService;
        _readOffDayService = readOffDayService;
        _readTransferPersonalService = readTransferPersonalService;
        _writeTransferPersonalService = writeTransferPersonalService;
        _transferPersonalExcelExport = transferPersonalExcelExport;
        _readMissingDayService = readMissingDayService;
        _writeMissingDayService = writeMissingDayService;
        _missingDayPersonalExcelExport = missingDayPersonalExcelExport;
    }

    #region PageActions
    /// <summary>
    /// Personel Detayları Sayfası
    /// </summary>
    /// <returns></returns>
    public IActionResult Edit(Guid id)
    {
        return View();
    }
    /// <summary>
    /// Personel İzinleri Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> PersonalOffDayList(OffdayQuery query)
    {
        var result = await _readOffDayService.GetPersonalOffDaysListService(query);
        return View(result);
    }
    /// <summary>
    /// Personel Nakil Listesi Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> PersonalTransferList(TransferPersonalQuery query)
    {
        var result = await _readTransferPersonalService.GetTransferPersonalListByIdService(query);
        return View(result);
    }
    /// <summary>
    /// Personel Eksik Gün Listesi Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> PersonalMissingDayList(MissingDayQuery query)
    {
        var result = await _readMissingDayService.GetMissingDayListByIdService(query);
        return View(result);
    }
    #endregion

    #region Get/Post Actions
    /// <summary>
    /// Personel Detayları Ajax Get Metodu
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> EditAjax(Guid id)
    {
        var result = await _readPersonalService.GetUpdatePersonalService(id);
        return Ok(result);
    }
    
    /// <summary>
    /// Personel Detayları Güncelleme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Edit(WriteUpdatePersonalDto dto)
    {
        IResultDto result = new ResultDto();
        if (!ModelState.IsValid)
        {
            result.SetStatus(false).SetErr("ModelState is not Valid").SetMessage("Lütfen Zorunlu Alanları Doğru Girdiğinize Emin Olunuz");
        }
        else
        {
            if (dto.RetiredOrOld == false && dto.IsYearLeaveRetired) return Ok(result.SetStatus(false).SetErr("ModelState is not valid").SetMessage("Yıllık izin yenilemesi emeklilik durumu baz alınsın istiyor iseniz personele ait emeklilik durumunu güncellemeniz gerekmektedir."));
            if (dto.RetiredOrOld && !dto.RetiredDate.HasValue) return Ok(result.SetStatus(false).SetErr("Personal retired but retired date is null").SetMessage("Emeklilik durumu açık ise emeklilik tarihi girmek zorunludur!"));
            result = await _writePersonalService.UpdateAsync(dto);
        }
        
        return Ok(result);
    }
    /// <summary>
    /// Personel Kümülatif Güncelleme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> UpdateCumulative(WriteUpdateCumulativeDto dto)
    {
        IResultDto result = new ResultDto();
        if (!ModelState.IsValid) return Ok(result.SetStatus(false).SetErr("Modelstate is not valid").SetMessage("Lütfen Zorunlu Alanların Girildiğinden Emin Olunuz."));
        if(dto.EarnedYearLeave < dto.RemainYearLeave) return Ok(result.SetStatus(false).SetErr("Math Error").SetMessage("Hak Edilen İzin, kalan izin miktarından küçük olamaz."));
        if(dto.EarnedYearLeave < 0 || dto.RemainYearLeave < 0) return Ok(result.SetStatus(false).SetErr("Math Error").SetMessage("Negatif Değer Girilemez!"));
        result = await _writePersonalService.UpdatePersonalCumulativeAsyncService(dto);
        
        
        return Ok(result);
    }
    /// <summary>
    /// Personel Kümülatif Güncelleme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> DeleteCumulative(Guid personalId,Guid cumulativeId)
    { 
        var result = await _writePersonalService.DeletePersonalCumulativeAsyncService(personalId, cumulativeId);
        return Ok(result);
    }
    /// <summary>
    /// Personel Sil Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ArchivePersonal(Guid id)
    {
        var result = await _writePersonalService.DeleteAsync(id);
        return Ok(result);
    }
    /// <summary>
    /// Personel Nakil Sil Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PersonalTransferDelete(Guid id)
    {
        var result = await _writeTransferPersonalService.DeleteTransferPersonalService(id);
        return Ok(result);
    }
    /// <summary>
    /// Personel Eksik Gün Ekleme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PersonalMissingDayAdd(WriteAddMissingDayDto dto)
    {
        IResultDto result = new ResultDto();
        if (!ModelState.IsValid)
        {
            result.SetStatus(false).SetErr("Modelstate is not valid").SetMessage("Lütfen Zorunlu Alanların Girildiğinden Emin Olunuz.");
        }
        else
        {
            result = await _writeMissingDayService.AddMissingDayService(dto);
        }
        
        return Ok(result);
    }
    /// <summary>
    /// Personel Eksik Gün Sil Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PersonalMissingDayDelete(Guid id)
    {
        var result = await _writeMissingDayService.DeleteMissingDayService(id);
        return Ok(result);
    }
    /// <summary>
    /// Personel İşten Çıkar veya İşe Al Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ChangeStatus(WritePersonalChangeStatusDto dto)
    {
        var result = await _writePersonalService.ChangeStatus(dto);
        return Ok(result);
    }
    /// <summary>
    /// Personel İzinleri Personel Header Ajax Get Metodu
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> PersonelDetailsHeader(Guid id)
    {
        var result = await _readPersonalService.GetPersonalDetailsHeaderByIdService(id);
        return Ok(result);
    }
    /// <summary>
    /// Personal Nakil Listesi Excel Raporu Alma
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PersonalTransferExportExcel(TransferPersonalQuery query , string returnUrl)
    {
        var result = await _readTransferPersonalService.ExcelGetTransferPersonalListByIdService(query);
        if (result.IsSuccess)
        {
            try
            {
                byte[] excelData = _transferPersonalExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.
                var response = HttpContext.Response;
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.Headers.Add($"Content-Disposition", $"attachment; filename=Nakil-Listesi.xlsx");
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
    /// Personal Eksik Gün Excel Raporu Alma
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PersonalMissingDayExportExcel(MissingDayQuery query , string returnUrl)
    {
        var result = await _readMissingDayService.ExcelGetPersonalMissingDayListByIdService(query);
        if (result.IsSuccess)
        {
            try
            {
                byte[] excelData = _missingDayPersonalExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.
                var response = HttpContext.Response;
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.Headers.Add($"Content-Disposition", $"attachment; filename=Eksik-Gun.xlsx");
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
    #endregion
  
   
   
}