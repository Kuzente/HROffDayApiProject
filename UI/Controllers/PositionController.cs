using Core.DTOs.PositionDTOs;
using Core.DTOs;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Services.Abstract.PositionServices;
using Services.ExcelDownloadServices.PositionServices;

namespace UI.Controllers
{
    [Authorize]
    public class PositionController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly IReadPositionService _readPositionService;
        private readonly IWritePositionService _writePositionService;
        private readonly PositionExcelExport _positionExcelExport;

        public PositionController(IReadPositionService readPositionService, IWritePositionService writePositionService, PositionExcelExport positionExcelExport, IToastNotification toastNotification)
        {
            _readPositionService = readPositionService;
            _writePositionService = writePositionService;
            _positionExcelExport = positionExcelExport;
            _toastNotification = toastNotification;
        }

        #region PageActions
        /// <summary>
        /// Ünvanlar Listesi Sayfası
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(PositionQuery query)
        {
            var resultSearch = await _readPositionService.GetPositionListService(query);
            if (!resultSearch.IsSuccess)
            {
                _toastNotification.AddErrorToastMessage(resultSearch.Message, new ToastrOptions { Title = "Hata" });
            }
            return View(resultSearch);
        }
        /// <summary>
        /// Ünvan Düzenle Sayfası 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UpdatePosition(Guid id, string returnUrl)
        {
            var result = await _readPositionService.GetUpdatePositionService(id);
            if (!result.IsSuccess)
            {
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            }
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
            var result = await _writePositionService.AddAsync(dto);
            if (!result.IsSuccess)
            {
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            }
            else
            {
                _toastNotification.AddSuccessToastMessage("Ünvan Başarılı Bir Şekilde Eklendi", new ToastrOptions { Title = "Başarılı" }); 
            }

            return Redirect(returnUrl);
        }
        /// <summary>
        /// Ünvan Düzenle Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdatePosition(ResultWithDataDto<PositionDto> dto, string returnUrl)
        {
            var result = await _writePositionService.UpdateAsync(dto.Data);
            if (!result.IsSuccess)
            {
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            }
            else
            {
                _toastNotification.AddSuccessToastMessage("Ünvan Başarılı Bir Şekilde Düzenlendi", new ToastrOptions { Title = "Başarılı" }); 
            }
            return Redirect(returnUrl);
        }
        /// <summary>
        /// Ünvan Sil Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ArchivePosition(Guid id, string returnUrl)
        {
            var result = await _writePositionService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            }
            else
            {
                _toastNotification.AddSuccessToastMessage("Ünvan Başarılı Bir Şekilde Silindi", new ToastrOptions { Title = "Başarılı" }); 
            }
            return Redirect(returnUrl);
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
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            return Redirect(returnUrl);
        }
        #endregion
       
       
    }
}
