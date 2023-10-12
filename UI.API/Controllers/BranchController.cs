using Core.DTOs.BranchDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;

namespace UI.API.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class BranchController : ControllerBase
	{
		private readonly IWriteBranchService _writeBranchService;
		private readonly IReadBranchService _readBranchService;

		public BranchController( IWriteBranchService writeBranchService, IReadBranchService readBranchService)
		{		
			_writeBranchService = writeBranchService;
			_readBranchService = readBranchService;
		}

		
		[HttpGet]
		public async Task<List<ReadBranchDto>> GetAll()
		{
			return await _readBranchService.GetAllAsync();
		} 
		
		
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] WriteBranchDto dto)
		{
			await _writeBranchService.AddAsync(dto);
			return Ok();
		}
		[HttpPut]
		public async Task<IActionResult> Update([FromBody] WriteBranchDto dto)
		{
			await _writeBranchService.UpdateAsync(dto);
			return Ok();
		}
		[HttpPut]
		public async Task<IActionResult> Archive([FromBody] int id)
		{
			await _writeBranchService.DeleteAsync(id);
			return Ok();
		}
		[HttpDelete]
		public async Task<IActionResult> Delete([FromBody] int id)
		{
			await _writeBranchService.RemoveAsync(id);
			return Ok();
		}
	}
}