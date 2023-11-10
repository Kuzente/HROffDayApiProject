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
        public async Task<IActionResult> Index(string search, bool passive, int pageNumber = 1)
        {
            var resultSearch = await _readPositionService.GetAllPagingOrderByAsync(pageNumber, search, passive);
            return View(resultSearch);
        }
        [HttpPost]
        public async Task<IActionResult> AddPosition(PositionDto dto, int pageNumber)
        {
            var result = await _writePositionService.AddAsync(dto);

            return RedirectToAction("Index", new { pageNumber = pageNumber });
        }
        [HttpGet]
        public async Task<IActionResult> UpdatePosition(int id, int pageNumber)
        {
            var result = await _readPositionService.GetByIdUpdate(id);

            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePosition(ResultWithDataDto<PositionDto> dto, int pageNumber = 1)
        {
            var result = await _writePositionService.UpdateAsync(dto.Data);

            return RedirectToAction("Index", new { pageNumber = pageNumber });
        }
        [HttpPost]
        public async Task<IActionResult> ArchivePosition(int id, int pageNumber = 1)
        {
            var result = await _writePositionService.DeleteAsync(id);
            if (result)
            {
                return RedirectToAction("Index", new { pageNumber = pageNumber });
            }

            return BadRequest("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ExportExcel()
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
            return RedirectToAction("Index");
        }
    }
}
