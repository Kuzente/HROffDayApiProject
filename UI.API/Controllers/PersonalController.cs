using Core.DTOs.PersonalDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.PersonalServices;

namespace UI.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PersonalController : ControllerBase
{
	private readonly IWritePersonalService _writePersonalService;
	private readonly IReadPersonalService _readPersonalService;

	public PersonalController(IWritePersonalService writePersonalService, IReadPersonalService readPersonalService)
	{
		_writePersonalService = writePersonalService;
		_readPersonalService = readPersonalService;
	}
	[HttpGet]
	public async Task<List<ReadPersonalDto>> GetAll()
	{
		return await _readPersonalService.GetAllAsync();
	}
	[HttpGet]
	public async Task<List<ReadPersonalDto>> GetAllWithBranch()
	{
		return await _readPersonalService.GetAllWithBranchAndPositionAsync();
	}


	[HttpPost]
	public async Task<IActionResult> Add([FromBody] WritePersonalDto dto)
	{
		await _writePersonalService.AddAsync(dto);
		return Ok();
	}
	[HttpPut]
	public async Task<IActionResult> Update([FromBody] WritePersonalDto dto)
	{
		await _writePersonalService.UpdateAsync(dto);
		return Ok();
	}
	[HttpPut]
	public async Task<IActionResult> Archive([FromBody] int id)
	{
		await _writePersonalService.DeleteAsync(id);
		return Ok();
	}
	[HttpDelete]
	public async Task<IActionResult> Delete([FromBody] int id)
	{
		await _writePersonalService.RemoveAsync(id);
		return Ok();
	}
}