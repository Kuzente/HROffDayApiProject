using Core;
using Core.DTOs.PersonalDTOs;
using Core.DTOs.PersonalDTOs.ReadDtos;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;
using Services.ExcelDownloadServices.PersonalServices;

namespace UI.Controllers
{
    [Authorize(Roles = nameof(UserRoleEnum.HumanResources))]
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
        /// <summary>
        /// Aktif Personeller Listesi
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index([FromQuery] PersonalQuery query)
        {
            var personals = await _readPersonalService.GetPersonalListService(query);
            return View(personals);
        }
        
        #endregion

        #region GET/POST Actions
        
        /// <summary>
        /// Aktif Şubeler ve Ünvanlar Select Ajax Get Metodu
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Personel Ekle Post Metodu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddPersonal(AddPersonalDto dto)
        {
            IResultDto result = new ResultDto();
            if (!ModelState.IsValid)
            {
                result.SetStatus(false).SetErr("ModelState is not Valid").SetErr("Zorunlu Alanları Doldurduğunuzdan Emin Olunuz");
            }
            else
            { 
                result = await _writePersonalService.AddAsync(dto);
            }
           
            return Ok(result);
        }
        /// <summary>
        /// Aktif Personeller Excel Alma Post Metodu
        /// </summary>
        /// <returns></returns>
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
            //_toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
            return Redirect("/personeller"+returnUrl);
        }
        #endregion
    }
}
