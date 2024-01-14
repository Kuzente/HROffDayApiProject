using Core.Querys;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Services.Abstract.BranchServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;
using Services.ExcelDownloadServices.PersonalServices;

namespace UI.Controllers;

public class PassivePersonalController : Controller
{
    private readonly IToastNotification _toastNotification;
    private readonly IReadPersonalService _readPersonalService;
    private readonly IReadBranchService _readBranchService;
    private readonly IReadPositionService _readPositionService;
    private readonly PassivePersonalExcelExport _passivePersonalExcelExport;
    

    public PassivePersonalController(IReadPersonalService readPersonalService, IReadBranchService readBranchService, IReadPositionService readPositionService, PassivePersonalExcelExport passivePersonalExcelExport, IToastNotification toastNotification)
    {
        _readPersonalService = readPersonalService;
        _readBranchService = readBranchService;
        _readPositionService = readPositionService;
        _passivePersonalExcelExport = passivePersonalExcelExport;
        _toastNotification = toastNotification;
    }

    #region PageActions

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PersonalQuery query)
    {
            
        var personals = await _readPersonalService.GetPassivePersonalListService(query);
        if (!personals.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(personals.Message, new ToastrOptions { Title = "Hata" });
        }
        ViewBag.Positions = await _readPositionService.GetAllJustNames();
        ViewBag.Branches = await _readBranchService.GetAllJustNames();
        return View(personals);
    }

    #endregion

    #region Get/Post Actions

    [HttpPost]
    public async Task<IActionResult> ExportExcel(PersonalQuery query,string returnUrl)
    {
        
        var result = await _readPersonalService.GetExcelPassivePersonalListService(query);
        if (result.IsSuccess)
        {
            byte[] excelData = _passivePersonalExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.

            var response = HttpContext.Response;
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.Headers.Add("Content-Disposition", "attachment; filename=CikarilanPersoneller.xlsx");
            await response.Body.WriteAsync(excelData, 0, excelData.Length);
            return new EmptyResult();
        }
        _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        return Redirect("cikarilan-personeller" + returnUrl);
    }

    #endregion
}