using Core.DTOs.PersonalDTOs;
using Core.DTOs.PersonalDTOs.ReadDtos;
using Core.Querys;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Services.Abstract.BranchServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;
using Services.ExcelDownloadServices.PersonalServices;

namespace UI.Controllers
{
    public class PersonalController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly IReadPersonalService _readPersonalService;
        private readonly IWritePersonalService _writePersonalService;
        private readonly IReadBranchService _readBranchService;
        private readonly IReadPositionService _readPositionService;
        private readonly PersonalExcelExport _personalExcelExport;

        public PersonalController(IReadPersonalService readPersonalService, IWritePersonalService writePersonalService, IReadBranchService readBranchService, IReadPositionService readPositionService, PersonalExcelExport personalExcelExport, IToastNotification toastNotification)
        {
            _readPersonalService = readPersonalService;
            _writePersonalService = writePersonalService;
            _readBranchService = readBranchService;
            _readPositionService = readPositionService;
            _personalExcelExport = personalExcelExport;
            _toastNotification = toastNotification;
        }

        #region PageActions
        public async Task<IActionResult> Index([FromQuery] PersonalQuery query)
        {
            
            var personals = await _readPersonalService.GetPersonalListService(query);
            if (!personals.IsSuccess)
            {
                _toastNotification.AddErrorToastMessage(personals.Message, new ToastrOptions { Title = "Hata" });
            }
            ViewBag.Positions = await _readPositionService.GetAllJustNames();
            ViewBag.Branches = await _readBranchService.GetAllJustNames();
            return View(personals);
        }
        
        #endregion

        #region GET/POST Actions
        [HttpPost]
        public async Task<IActionResult> ExportExcel(PersonalQuery query, string returnUrl)
        {
            //PersonalQuery queryGet = System.Text.Json.JsonSerializer.Deserialize<PersonalQuery>(query);
            var result = await _readPersonalService.GetExcelPersonalListService(query);
            if (result.IsSuccess)
            {
                byte[] excelData = _personalExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.

                var response = HttpContext.Response;
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.Headers.Add("Content-Disposition", "attachment; filename=Personeller.xlsx");
                await response.Body.WriteAsync(excelData, 0, excelData.Length);
                return new EmptyResult();
            }
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            return Redirect("/personeller"+returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> GetBranchAndPositions()
        {
            var branches = await _readBranchService.GetAllJustNames();
            var positions = await _readPositionService.GetAllJustNames();
            
            var dto = new ReadCreatePersonalBranchesPositionsDto
            {
                Branches = branches,
                Positions = positions 
            };
            return Ok(dto);
        }
        [HttpPost]
        public async Task<IActionResult> AddPersonal(AddPersonalDto dto)
        {
            var result = await _writePersonalService.AddAsync(dto);
            if (!result.IsSuccess)
            {
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            }
            else
            {
                _toastNotification.AddSuccessToastMessage("Personel Başarılı Bir Şekilde Eklendi", new ToastrOptions { Title = "Başarılı" }); 
            }
            return Ok(result);
        }
        #endregion
        
      
        
    }
}
