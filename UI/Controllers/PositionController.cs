using Core;
using Core.DTOs.PositionDTOs;
using Core.DTOs;
using Core.Interfaces;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.PositionServices;
using Services.ExcelDownloadServices.PositionServices;

namespace UI.Controllers
{
    [Authorize]
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

        #region PageActions
        /// <summary>
        /// Ünvanlar Listesi Sayfası
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(PositionQuery query)
        {
            var resultSearch = await _readPositionService.GetPositionListService(query);
            return View(resultSearch);
        }
        /// <summary>
        /// Ünvan Düzenle Sayfası 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UpdatePosition(Guid id, string returnUrl)
        {
            var result = await _readPositionService.GetUpdatePositionService(id);
            ViewData["ReturnUrl"] = returnUrl; 
            return View(result);
        }
        #endregion

        #region Get/PostActions
        /// <summary>
        /// Ünvan Ekle Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddPosition(PositionDto dto, string returnUrl)
        {
            IResultDto result = new ResultDto();
            if (!ModelState.IsValid)
            {
                result.SetStatus(false).SetErr("Modelstate is not valid").SetMessage("Lütfen Zorunlu Alanları Girdiğinize Emin Olunuz.");
            }
            else
            {
                result = await _writePositionService.AddAsync(dto); 
            }
            return Ok(result);
        }
        /// <summary>
        /// Ünvan Düzenle Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdatePosition(ResultWithDataDto<PositionDto> dto, string returnUrl)
        {
            IResultWithDataDto<PositionDto> result = new ResultWithDataDto<PositionDto>();
            if (!ModelState.IsValid)
            {
                result.SetStatus(false).SetErr("Modelstate is not valid").SetMessage("Lütfen Zorunlu Alanları Girdiğinize Emin Olunuz.");
            }
            else
            {
                result = await _writePositionService.UpdateAsync(dto.Data);
            }
           
            return Ok(result);
        }
        /// <summary>
        /// Ünvan Sil Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ArchivePosition(Guid id, string returnUrl)
        {
            var result = await _writePositionService.DeleteAsync(id);
            return Ok(result);
        }
        /// <summary>
        /// Ünvanlar Listesi Excel Alma Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ExportExcel(PositionQuery query,string returnUrl)
        {
           
            var result = await _readPositionService.GetExcelPositionListService(query);
            if (result.IsSuccess)
            {
                byte[] excelData = _positionExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.

                var response = HttpContext.Response;
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.Headers.Add("Content-Disposition", "attachment; filename=Unvanlar.xlsx");
                await response.Body.WriteAsync(excelData, 0, excelData.Length);
                return new EmptyResult();
            }
            //_toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            return Redirect(returnUrl);
        }
        #endregion
       
       
    }
}
