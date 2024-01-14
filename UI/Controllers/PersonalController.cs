using Core.DTOs.PersonalDTOs;
using Core.DTOs.PersonalDTOs.ReadDtos;
using Core.Querys;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;
using Services.ExcelDownloadServices.PersonalServices;

namespace UI.Controllers
{
    public class PersonalController : Controller
    {
        private readonly IReadPersonalService _readPersonalService;
        private readonly IWritePersonalService _writePersonalService;
        private readonly IReadBranchService _readBranchService;
        private readonly IReadPositionService _readPositionService;
        private readonly PersonalExcelExport _personalExcelExport;

        public PersonalController(IReadPersonalService readPersonalService, IWritePersonalService writePersonalService, IReadBranchService readBranchService, IReadPositionService readPositionService, PersonalExcelExport personalExcelExport)
        {
            _readPersonalService = readPersonalService;
            _writePersonalService = writePersonalService;
            _readBranchService = readBranchService;
            _readPositionService = readPositionService;
            _personalExcelExport = personalExcelExport;
        }

        #region PageActions
        public async Task<IActionResult> Index([FromQuery] PersonalQuery query)
        {
            
            var personals = await _readPersonalService.GetAllPagingWithBranchAndPositionOrderByAsync(query);
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
            var result = await _readPersonalService.GetAllWithFilterAsync(query);
            if (result.IsSuccess)
            {
                byte[] excelData = _personalExcelExport.ExportToExcel(result.Data); // Entity listesini Excel verisi olarak alın.

                var response = HttpContext.Response;
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.Headers.Add("Content-Disposition", "attachment; filename=Personeller.xlsx");
                await response.Body.WriteAsync(excelData, 0, excelData.Length);
                return new EmptyResult();
            }
            return Redirect("/personeller"+returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> GetBranchAndPositions()
        {
            var dto = new ReadCreatePersonalBranchesPositionsDto
            {
                Branches = await _readBranchService.GetAllJustNames(),
                Positions = await _readPositionService.GetAllJustNames()
            };
            return Ok(dto);
        }
        [HttpPost]
        public async Task<IActionResult> AddPersonal(AddPersonalDto dto)
        {
            var result = await _writePersonalService.AddAsync(dto);
            if (!result.IsSuccess)
            {
                //Error Page TODO
            }
            return Ok(result);
        }
        #endregion
        
      
        
    }
}
