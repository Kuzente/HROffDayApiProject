using Core.DTOs.BranchDTOs;
using Core.DTOs;
using Core.DTOs.BaseDTOs;
using Core.Querys;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.ExcelDownloadServices.BranchServices;
using UI.Models;

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

        #region PageActions
        public async Task<IActionResult> Index(string search, bool passive, int sayfa = 1)
        {
            var resultSearch = await _readBranchService.GetBranchListService(sayfa, search , passive);
            return View(resultSearch);
        }
        public async Task<IActionResult> UpdateBranch(Guid id, string returnUrl)
        {
            var result = await _readBranchService.GetUpdateBranchService(id);
            ViewData["ReturnUrl"] = returnUrl;
            return View(result);
        }

        #endregion

        #region Get/Post Actions
        [HttpPost]
        public async Task<IActionResult> ExportExcel(BranchQuery query,string returnUrl)
        {
            
            var result = await _readBranchService.GetExcelBranchListService(query);
            if (result.IsSuccess)
            {
                byte[] excelData = _branchExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.
            
                var response = HttpContext.Response;
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.Headers.Add("Content-Disposition", "attachment; filename=Subeler.xlsx");
                await response.Body.WriteAsync(excelData, 0, excelData.Length);
                return new EmptyResult();
            }

            return Redirect("/subeler"+returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> AddBranch(BranchDto dto, string returnUrl)
        {
            var result = await _writeBranchService.AddAsync(dto);
            if (!result.IsSuccess)
            {
                //Error Page TODO
            }

            return Redirect("/subeler"+returnUrl);
        }
      
        [HttpPost]
        public async Task<IActionResult> UpdateBranch(ResultWithDataDto<BranchDto> dto, string returnUrl)
        {
            var result = await _writeBranchService.UpdateAsync(dto.Data);

            return Redirect("/subeler"+returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> ArchiveBranch(Guid id, string returnUrl)
        {
            var result = await _writeBranchService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                //Error Page TODO
            }

            return Redirect("/subeler"+returnUrl);
        }
        #endregion
      
      
    }
}
