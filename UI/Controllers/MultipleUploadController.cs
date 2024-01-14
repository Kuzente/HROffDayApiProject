using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;
using Services.ExcelDownloadServices;
using Services.FileUpload;

namespace UI.Controllers;

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

    [HttpGet]
    public IActionResult PersonalUpload()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> PersonalUpload(IFormFile file)
    {
        var list =  _excelPersonalAddrange.ImportDataFromExcel(file);
        var result = await _writePersonalService.AddRangeAsync(list);
        if (!result.IsSuccess)
        {
            //Error Sayfası Yönlendir TODO
        }
        return View();
    }
    public async Task<IActionResult> GetExcelSheme()
    {
        // var branches = await _readBranchService.GetAllOrderByAsync();
        // var positions = await _readPositionService.GetAllOrderByAsync();
        // if (branches.IsSuccess && positions.IsSuccess)
        // {
        //     byte[] excelData = _excelUploadScheme.ExportToExcel(positions.Data,branches.Data); // Entity listesini Excel verisi olarak alın.
        //
        //     var response = HttpContext.Response;
        //     response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //     response.Headers.Add("Content-Disposition", "attachment; filename=TopluVeriTaslak.xlsx");
        //     await response.Body.WriteAsync(excelData, 0, excelData.Length);
        //     return new EmptyResult();
        // }
        return Redirect("/personal-upload");
    }
}