using Core.DTOs.PositionDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.PositionServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UI.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PositionController : ControllerBase
{
	private readonly IReadPositionService _readPositionService;
	private readonly IWritePositionService _writePositionService;

	public PositionController(IReadPositionService readPositionService, IWritePositionService writePositionService)
	{
		_readPositionService = readPositionService;
		_writePositionService = writePositionService;
	}

	[HttpGet]
	public async Task<List<ReadPositionDto>> GetAll()
	{
		return await _readPositionService.GetAllAsync();
	}
	[HttpGet]
	public async Task<IActionResult> GetAny(string name)
	{
		var result =  await _readPositionService.GetAnyByNameAsync(name);
		if (result)
		{
			return Ok();
		}
		return NotFound();
	}


	[HttpPost]
	public async Task<IActionResult> Add([FromBody] WritePositionDto dto)
	{
		await _writePositionService.AddAsync(dto);
		return Ok();
	}
	[HttpPut]
	public async Task<IActionResult> Update([FromBody] WritePositionDto dto)
	{
		await _writePositionService.UpdateAsync(dto);
		return Ok();
	}
	[HttpPut]
	public async Task<IActionResult> Archive([FromBody] int id)
	{
		await _writePositionService.DeleteAsync(id);
		return Ok();
	}
	[HttpDelete]
	public async Task<IActionResult> Delete([FromBody] int id)
	{
		await _writePositionService.RemoveAsync(id);
		return Ok();
	}
}