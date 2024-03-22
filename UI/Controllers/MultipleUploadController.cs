using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.ExcelServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;
using Services.ExcelDownloadServices;
using Services.FileUpload;

namespace UI.Controllers;

[Authorize]
public class MultipleUploadController : Controller
{
    private readonly ExcelPersonalAddrange _excelPersonalAddrange;
    private readonly IWritePersonalService _writePersonalService;
    private readonly IReadBranchService _readBranchService;
    private readonly IReadPositionService _readPositionService;
    private readonly ExcelUploadScheme _excelUploadScheme;
    private readonly IReadExcelServices _readExcelServices;

    public MultipleUploadController(ExcelPersonalAddrange excelPersonalAddrange,
        IWritePersonalService writePersonalService, IReadPositionService readPositionService,
        IReadBranchService readBranchService, ExcelUploadScheme excelUploadScheme, IReadExcelServices readExcelServices)
    {
        _excelPersonalAddrange = excelPersonalAddrange;
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

        byte[] excelData =
            _excelUploadScheme.ExportToExcel(positions, branches); // Entity listesini Excel verisi olarak alın.

        var response = HttpContext.Response;
        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        response.Headers.Add("Content-Disposition", "attachment; filename=TopluVeriTaslak.xlsx");
        await response.Body.WriteAsync(excelData, 0, excelData.Length);
        return new EmptyResult();

        return Redirect("/toplu-islemler");
    }

    /// <summary>
    /// Toplu Personel Ekleme Post Metodu 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PersonalUpload(IFormFile file)
    {
        //var list = _excelPersonalAddrange.ImportDataFromExcel(file);
        var resultExcel = await _readExcelServices.ImportDataFromExcel(file);
        if (!resultExcel.IsSuccess)
            return Ok(resultExcel);
        var result = await _writePersonalService.AddRangeAsync(resultExcel.Data);
        

        return Ok(result);
    }

    #endregion
}