using Core.DTOs.PositionDTOs;
using Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.PositionServices;
using Services.ExcelDownloadServices.PositionServices;

namespace UI.Controllers
{
    public class PositionController : Controller
    {
        private readonly IReadPositionService _readPositionService;
        private readonly IWritePositionService _writePositionService;
        private readonly PositionExcelExport _positionExcelExport;

        public PositionController(IReadPositionService readPositionService, IWritePositionService writePositionService, PositionExcelExport positionExcelExport)
        {
            _readPositionService = readPositionService;
            _writePositionService = writePositionService;
            _positionExcelExport = positionExcelExport;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, bool passive, int sayfa = 1)
        {
            var resultSearch = await _readPositionService.GetAllPagingOrderByAsync(sayfa, search, passive);
            return View(resultSearch);
        }
        [HttpPost]
        public async Task<IActionResult> AddPosition(PositionDto dto, string returnUrl)
        {
            var result = await _writePositionService.AddAsync(dto);
            if (!result.IsSuccess)
            {
                //Error Page TODO
            }

            return Redirect("/unvanlar"+returnUrl);
        }
        [HttpGet]
        public async Task<IActionResult> UpdatePosition(Guid id, string returnUrl)
        {
            var result = await _readPositionService.GetByIdUpdate(id);
            ViewData["ReturnUrl"] = returnUrl; 
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePosition(ResultWithDataDto<PositionDto> dto, string returnUrl)
        {
            var result = await _writePositionService.UpdateAsync(dto.Data);

            return Redirect("/unvanlar"+returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> ArchivePosition(Guid id, string returnUrl)
        {
            var result = await _writePositionService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                //Error Page TODO
            }
            return Redirect("/unvanlar"+returnUrl);
        }
        [HttpGet]
        public async Task<IActionResult> ExportExcel(string returnUrl)
        {
           
            var result = await _readPositionService.GetAllOrderByAsync();
            if (result.IsSuccess)
            {
                byte[] excelData = _positionExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.

                var response = HttpContext.Response;
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.Headers.Add("Content-Disposition", "attachment; filename=Unvanlar.xlsx");
                await response.Body.WriteAsync(excelData, 0, excelData.Length);
                return new EmptyResult();
            }
            return Redirect("/unvanlar"+returnUrl);
        }
    }
}
