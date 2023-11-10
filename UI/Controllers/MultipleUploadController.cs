using Microsoft.AspNetCore.Mvc;
using Services.Abstract.PersonalServices;
using Services.FileUpload;

namespace UI.Controllers;

public class MultipleUploadController : Controller
{
    private readonly ExcelPersonalAddrange _excelPersonalAddrange;
    private readonly IWritePersonalService _writePersonalService;

    public MultipleUploadController(ExcelPersonalAddrange excelPersonalAddrange, IWritePersonalService writePersonalService)
    {
        _excelPersonalAddrange = excelPersonalAddrange;
        _writePersonalService = writePersonalService;
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
}