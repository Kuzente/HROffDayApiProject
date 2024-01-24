using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Services.Abstract.BranchServices;
using Services.ExcelDownloadServices.BranchServices;

namespace UI.Controllers
{
    [Authorize]
    public class BranchController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly IReadBranchService _readBranchService;
        private readonly IWriteBranchService _writeBranchService;
        private readonly BranchExcelExport _branchExcelExport;

        public BranchController(IReadBranchService readBranchService, IWriteBranchService writeBranchService, BranchExcelExport branchExcelExport, IToastNotification toastNotification)
        {
            _readBranchService = readBranchService;
            _writeBranchService = writeBranchService;
            _branchExcelExport = branchExcelExport;
            _toastNotification = toastNotification;
        }

        #region PageActions
        /// <summary>
        /// Şubeler Listesi Sayfası 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(BranchQuery query)
        {
           
            var resultSearch = await _readBranchService.GetBranchListService(query);
            if (!resultSearch.IsSuccess)
            {
                _toastNotification.AddErrorToastMessage(resultSearch.Message, new ToastrOptions { Title = "Hata" });
            }
            return View(resultSearch);
        }
        /// <summary>
        /// Şube Güncelle Sayfası
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UpdateBranch(Guid id, string returnUrl)
        {
            var result = await _readBranchService.GetUpdateBranchService(id);
            if (!result.IsSuccess)
            {
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            }
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
            var result = await _writeBranchService.AddAsync(dto);
            if (!result.IsSuccess)
            {
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            }
            else
            {
                _toastNotification.AddSuccessToastMessage("Şube Başarılı Bir Şekilde Eklendi", new ToastrOptions { Title = "Başarılı" });
            }

            return Redirect(returnUrl);
        }
        /// <summary>
        /// Şube Düzenleme Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateBranch(ResultWithDataDto<BranchDto> dto, string returnUrl)
        {
            var result = await _writeBranchService.UpdateAsync(dto.Data);
            if (!result.IsSuccess)
            {
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            }
            else
            {
                _toastNotification.AddSuccessToastMessage("Şube Başarılı Bir Şekilde Düzenlendi", new ToastrOptions { Title = "Başarılı" });
            }
            return Redirect(returnUrl);
        }
        /// <summary>
        /// Şube Silme Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ArchiveBranch(Guid id, string returnUrl)
        {
            var result = await _writeBranchService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            }
            else
            {
                _toastNotification.AddSuccessToastMessage("Şube Başarılı Bir Şekilde Silindi", new ToastrOptions { Title = "Başarılı" });
            }
            return Redirect(returnUrl);
        }
        /// <summary>
        /// Şube Listesi Excel Alma Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ExportExcel(BranchQuery query, string returnUrl)
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

            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            return Redirect(returnUrl);
        }
        #endregion


    }
}
