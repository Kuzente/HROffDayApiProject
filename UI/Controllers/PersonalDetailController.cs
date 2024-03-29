using Core;
using Core.DTOs.PersonalDetailDto.WriteDtos;
using Core.DTOs.PersonalDTOs.WriteDtos;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.OffDayServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;
using Services.Abstract.TransferPersonalService;
using Services.ExcelDownloadServices.TransferPersonalServices;

namespace UI.Controllers;

[Authorize(Roles = nameof(UserRoleEnum.HumanResources))]
public class PersonalDetailController : Controller
{
    private readonly IReadPersonalService _readPersonalService;
    private readonly IReadOffDayService _readOffDayService;
    private readonly IWritePersonalService _writePersonalService;
    private readonly IReadTransferPersonalService _readTransferPersonalService;
    private readonly IWriteTransferPersonalService _writeTransferPersonalService;
    private readonly TransferPersonalExcelExport _transferPersonalExcelExport;

    public PersonalDetailController(IReadPersonalService readPersonalService, IWritePersonalService writePersonalService,IReadOffDayService readOffDayService, IReadTransferPersonalService readTransferPersonalService, IWriteTransferPersonalService writeTransferPersonalService, TransferPersonalExcelExport transferPersonalExcelExport)
    {
        _readPersonalService = readPersonalService;
        _writePersonalService = writePersonalService;
        _readOffDayService = readOffDayService;
        _readTransferPersonalService = readTransferPersonalService;
        _writeTransferPersonalService = writeTransferPersonalService;
        _transferPersonalExcelExport = transferPersonalExcelExport;
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
            result = await _writePersonalService.UpdateAsync(dto);
        }
        
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
    [Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)}")]
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
    #endregion
  
   
   
}