﻿using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.ExcelServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;
using Services.ExcelDownloadServices;
using UI.Helpers;

namespace UI.Controllers;

[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
public class MultipleUploadController : BaseController
{
    private readonly IWritePersonalService _writePersonalService;
    private readonly IReadBranchService _readBranchService;
    private readonly IReadPositionService _readPositionService;
    private readonly ExcelUploadScheme _excelUploadScheme;
    private readonly IReadExcelServices _readExcelServices;

    public MultipleUploadController(
        IWritePersonalService writePersonalService, IReadPositionService readPositionService,
        IReadBranchService readBranchService, ExcelUploadScheme excelUploadScheme, IReadExcelServices readExcelServices)
    {
        _writePersonalService = writePersonalService;
        _readPositionService = readPositionService;
        _readBranchService = readBranchService;
        _excelUploadScheme = excelUploadScheme;
        _readExcelServices = readExcelServices;
    }

    #region PageActions

    /// <summary>
    /// Toplu Personel Ekleme Sayfası
    /// </summary>
    /// <returns></returns>
    public IActionResult PersonalUpload()
    {
        return View();
    }

    #endregion

    #region Get/Post Methods

    /// <summary>
    /// Toplu Personel Ekleme Taslak Excel İndirme Metodu
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> GetExcelSheme()
    {
        var branches = await _readBranchService.GetAllJustNames();
        var positions = await _readPositionService.GetAllJustNames();
        string personalStaticSelects = PersonalStaticSelectsConverter.JsonToString();

        byte[] excelData =
            _excelUploadScheme.ExportToExcel(positions, branches , personalStaticSelects); // Entity listesini Excel verisi olarak alın.

        var response = HttpContext.Response;
        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        response.Headers.Add("Content-Disposition", "attachment; filename=TopluVeriTaslak.xlsx");
        await response.Body.WriteAsync(excelData, 0, excelData.Length);
        return new EmptyResult();
    }

    /// <summary>
    /// Toplu Personel Ekleme Post Metodu 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PersonalUpload(IFormFile file)
    {
        var resultExcel = await _readExcelServices.ImportDataFromExcel(file);
        if (!resultExcel.IsSuccess) return Ok(resultExcel);
        if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
        var result = await _writePersonalService.AddRangeAsync(resultExcel.Data,GetClientUserId()!.Value,GetClientIpAddress());
        

        return Ok(result);
    }

    #endregion
}