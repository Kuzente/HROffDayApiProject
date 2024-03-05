using Core;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Interfaces;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.ExcelDownloadServices.BranchServices;

namespace UI.Controllers
{
    [Authorize]
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
        /// <summary>
        /// Şubeler Listesi Sayfası 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(BranchQuery query)
        {
            var resultSearch = await _readBranchService.GetBranchListService(query);
            return View(resultSearch);
        }
        /// <summary>
        /// Şube Güncelle Sayfası
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UpdateBranch(Guid id, string returnUrl)
        {
            var result = await _readBranchService.GetUpdateBranchService(id);
            ViewData["ReturnUrl"] = returnUrl;
            return View(result);
        }

        #endregion

        #region Get/Post Actions
        /// <summary>
        /// Şube Ekleme Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddBranch(BranchDto dto, string returnUrl)
        {
            IResultDto result = new ResultDto();
            if (!ModelState.IsValid)
            {
                result.SetStatus(false).SetErr("Modelstate is not valid").SetMessage("Lütfen Zorunlu Alanların Girildiğinden Emin Olunuz.");
            }
            else
            { 
                result = await _writeBranchService.AddAsync(dto); 
            }
            
            return Ok(result);
        }
        /// <summary>
        /// Şube Düzenleme Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateBranch(ResultWithDataDto<BranchDto> dto, string returnUrl)
        {
            IResultWithDataDto<BranchDto> result = new ResultWithDataDto<BranchDto>();
            if (!ModelState.IsValid)
            {
                result.SetStatus(false).SetErr("Modelstate is not valid").SetMessage("Lütfen Zorunlu Alanların Girildiğinden Emin Olunuz.");
            }
            else
            {
                result = await _writeBranchService.UpdateAsync(dto.Data);
            }
           
            return Ok(result);
        }
        /// <summary>
        /// Şube Silme Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ArchiveBranch(Guid id, string returnUrl)
        {
            var result = await _writeBranchService.DeleteAsync(id);
            return Ok(result);
        }
        /// <summary>
        /// Şube Listesi Excel Alma Post Metodu
        /// </summary>
        /// <returns></returns>
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
                // _toastNotification.AddSuccessToastMessage("Başarılı", new ToastrOptions { Title = "Başarılı" });
            }
            
            // _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            return Redirect(returnUrl);
        }
        #endregion


    }
}
