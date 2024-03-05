using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
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

    public MultipleUploadController(ExcelPersonalAddrange excelPersonalAddrange, IWritePersonalService writePersonalService, IReadPositionService readPositionService, IReadBranchService readBranchService, ExcelUploadScheme excelUploadScheme)
    {
        _excelPersonalAddrange = excelPersonalAddrange;
        _writePersonalService = writePersonalService;
        _readPositionService = readPositionService;
        _readBranchService = readBranchService;
        _excelUploadScheme = excelUploadScheme;
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
        
            byte[] excelData = _excelUploadScheme.ExportToExcel(positions,branches); // Entity listesini Excel verisi olarak alın.
        
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
        try
        {
            var list = _excelPersonalAddrange.ImportDataFromExcel(file);
            var result = await _writePersonalService.AddRangeAsync(list);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return View();
    }
    #endregion
}