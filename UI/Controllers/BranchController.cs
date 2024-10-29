using Core;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.ExcelDownloadServices.BranchServices;
using Services.ExcelDownloadServices.PersonalCountsServices;

namespace UI.Controllers
{
    [Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
    public class BranchController : BaseController
    {
        private readonly IReadBranchService _readBranchService;
        private readonly IWriteBranchService _writeBranchService;
        private readonly BranchExcelExport _branchExcelExport;
        private readonly PersonalCountExcelExport _personalCountExcelExport;

		public BranchController(IReadBranchService readBranchService, IWriteBranchService writeBranchService, BranchExcelExport branchExcelExport, PersonalCountExcelExport personalCountExcelExport)
		{
			_readBranchService = readBranchService;
			_writeBranchService = writeBranchService;
			_branchExcelExport = branchExcelExport;
			_personalCountExcelExport = personalCountExcelExport;
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
        public async Task<IActionResult> AddBranch(BranchDto dto)
        {
            IResultDto result = new ResultDto();
            if (!ModelState.IsValid)
            {
                result.SetStatus(false).SetErr("Modelstate is not valid").SetMessage("Lütfen Zorunlu Alanların Girildiğinden Emin Olunuz.");
            }
            else
            { 
                if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
                //result = await _writeBranchService.AddAsync(dto,GetClientUserId()!.Value,GetClientIpAddress()); 
                var task1 =  _writeBranchService.AddAsync(dto,GetClientUserId()!.Value,GetClientIpAddress()); 
                var task2 = _writeBranchService.AddAsync(dto,GetClientUserId()!.Value,GetClientIpAddress());
                var result2 = await Task.WhenAll(task1, task2);
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
                if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
                result = await _writeBranchService.UpdateAsync(dto.Data,GetClientUserId()!.Value,GetClientIpAddress());
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
            if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
            var result = await _writeBranchService.DeleteAsync(id,GetClientUserId()!.Value,GetClientIpAddress());
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
            }
            
            return Redirect(returnUrl);
        }
		
		public async Task<IActionResult> ExportExcelDepartment(string returnUrl)
		{

			var result = await _readBranchService.GetDepartmantCountsByBranchService();
			if (result.IsSuccess)
			{
				byte[] excelData = _personalCountExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.
				var response = HttpContext.Response;
				response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				response.Headers.Add("Content-Disposition", "attachment; filename=NormKadro.xlsx");
				await response.Body.WriteAsync(excelData, 0, excelData.Length);
				return new EmptyResult();
			}
			return Redirect("404");
		}
		#endregion


	}
}
