using Core.DTOs.BranchDTOs;
using Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.ExcelDownloadServices.BranchServices;

namespace UI.Controllers
{
    public class BranchController : Controller
    {
        private readonly IReadBranchService _readBranchService;
        private readonly IWriteBranchService _writeBranchService;
        private readonly BranchExcelExport _branchExcelExport;

        public BranchController(IReadBranchService readBranchService, IWriteBranchService writeBranchService, BranchExcelExport branchExcelExport)
        {
            _readBranchService = readBranchService;
            _writeBranchService = writeBranchService;
            _branchExcelExport = branchExcelExport;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, bool passive, int pageNumber = 1)
        {
            var resultSearch = await _readBranchService.GetAllPagingOrderByAsync(pageNumber, search , passive);
            return View(resultSearch);
        }

        [HttpPost]
        public async Task<IActionResult> AddBranch(BranchDto dto, int pageNumber)
        {
            var result = await _writeBranchService.AddAsync(dto);

            return RedirectToAction("Index", new { pageNumber = pageNumber });
        }
        [HttpGet]
        public async Task<IActionResult> UpdateBranch(int id, int pageNumber)
        {
            var result = await _readBranchService.GetByIdUpdate(id);

            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBranch(ResultWithDataDto<BranchDto> dto, int pageNumber = 1)
        {
            var result = await _writeBranchService.UpdateAsync(dto.Data);

            return RedirectToAction("Index", new { pageNumber = pageNumber });
        }
        [HttpPost]
        public async Task<IActionResult> ArchiveBranch(int id, int pageNumber = 1)
        {
            var result = await _writeBranchService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                //Error Page TODO
            }

            return RedirectToAction("Index", new { pageNumber = pageNumber });
        }
        [HttpGet]
        public async Task<IActionResult> ExportExcel()
        {
            
            var result = await _readBranchService.GetAllOrderByAsync();
            if (result.IsSuccess)
            {
                byte[] excelData = _branchExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.

                var response = HttpContext.Response;
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.Headers.Add("Content-Disposition", "attachment; filename=Subeler.xlsx");
                await response.Body.WriteAsync(excelData, 0, excelData.Length);
                return new EmptyResult();
            }
            return RedirectToAction("Index");
        }
    }
}
