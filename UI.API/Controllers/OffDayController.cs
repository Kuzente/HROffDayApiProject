using Core.DTOs.OffDayDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.OffDayServices;

namespace UI.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class OffDayController : ControllerBase
{
    private readonly IWriteOffDayService _writeOffDayService;
    private readonly IReadOffDayService _readOffDayService;
    public OffDayController(IWriteOffDayService writeOffDayService, IReadOffDayService readOffDayService)
    {
        _writeOffDayService = writeOffDayService;
        _readOffDayService = readOffDayService;
    }
    [HttpGet]
    public async Task<List<ReadOffDayDto>> GetAll()
    {
        return await _readOffDayService.GetAllAsync();
    }
    [HttpGet]
    public async Task<List<ReadOffDayDto>> GetAllWithPersonal()
    {
        return await _readOffDayService.GetAllWithPersonal();
    }


    [HttpPost]
    public async Task<IActionResult> Add([FromBody] WriteOffDayDto dto)
    {
        await _writeOffDayService.AddAsync(dto);
        return Ok();
    }
    [HttpPut]
    public async Task<IActionResult> ChangeStatus([FromBody] int id, bool isapproved)
    {
        await _writeOffDayService.ChangeOffDayStatus(id,isapproved);
        return Ok();
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] WriteOffDayDto dto)
    {
        var result = await _writeOffDayService.UpdateAsync(dto);
        return Ok(result);
    }
    [HttpPut]
    public async Task<IActionResult> Archive([FromBody] int id)
    {
        await _writeOffDayService.DeleteAsync(id);
        return Ok();
    }
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] int id)
    {
        await _writeOffDayService.RemoveAsync(id);
        return Ok();
    }
        
        
}