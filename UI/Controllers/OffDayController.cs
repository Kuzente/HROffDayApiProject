using Core.DTOs.OffDayDTOs;
using Core.DTOs.PersonalDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.OffDayServices;
using Services.Abstract.PersonalServices;

namespace UI.Controllers;

public class OffDayController : Controller
{
    private readonly IReadPersonalService _readPersonalService;
    private readonly IWriteOffDayService _writeOffDayService;

    public OffDayController(IReadPersonalService readPersonalService, IWriteOffDayService writeOffDayService)
    {
        _readPersonalService = readPersonalService;
        _writeOffDayService = writeOffDayService;
    }

    [HttpGet]
    public async Task<IActionResult> AddOffDay(Guid id)
    {
        AddOffdayDto dto = new AddOffdayDto();
        var personalResult = await _readPersonalService.GetAllPersonalByBranchId(id);
        if (!personalResult.IsSuccess)
        {
            //Error Sayfası TODO
        }

        dto.PersonalOffDayDtos = personalResult.Data;
        return View(dto);
    }
    [HttpPost]
    public async Task<IActionResult> AddOffDay(AddOffdayDto dto,int branchId)
    {
        //if (dto.LeaveByYear>0 && dto.LeaveByYear > (dto.PersonalOffDayDtos.))
        //{
            
        //}
        var personalResult = await _writeOffDayService.AddAsync(dto);
        if (!personalResult.IsSuccess)
        {
            //Error Sayfası TODO
        }

       
        return RedirectToAction("AddOffDay", new {id = branchId});
    }
}