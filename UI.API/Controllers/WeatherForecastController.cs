using Core.DTOs.BranchDTOs;
using Core.DTOs.PersonalDTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.PersonalServices;

namespace UI.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		

		private readonly ILogger<WeatherForecastController> _logger;
		private readonly IWritePersonalService  _writePersonalService;
		private readonly IReadPersonalService _readPersonalService;
		private readonly IWriteBranchService _writeBranchService;
		private readonly IReadBranchService _readBranchService;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IWritePersonalService writePersonalService, IReadPersonalService readPersonalService, IWriteBranchService writeBranchService, IReadBranchService readBranchService)
		{
			_logger = logger;
			_writePersonalService = writePersonalService;
			_readPersonalService = readPersonalService;
			_writeBranchService = writeBranchService;
			_readBranchService = readBranchService;
		}

		
		[HttpGet("GetAllBranch")]
		public async Task<List<ReadBranchDto>> GetAllBranch()
		{
			return await _readBranchService.GetAllAsync();
		} 
		
		[HttpPost("AddPersonal")]
		public async Task<IActionResult> Post([FromBody] WritePersonalDto dto)
		{
			await _writePersonalService.AddAsync(dto);
			return Ok();

		}
		[HttpPost("AddBranch")]
		public async Task<IActionResult> Post([FromBody] WriteBranchDto dto)
		{
			await _writeBranchService.AddAsync(dto);
			return Ok();
		}
		[HttpPut("UpdateBranch")]
		public async Task<IActionResult> Put([FromBody] WriteBranchDto dto)
		{
			await _writeBranchService.UpdateAsync(dto);
			return Ok();
		}
		[HttpPut("ArchiveBranch")]
		public async Task<IActionResult> Put([FromBody] int id)
		{
			await _writeBranchService.DeleteAsync(id);
			return Ok();
		}
		[HttpDelete("RemoveBranch")]
		public async Task<IActionResult> Delete([FromBody] int id)
		{
			await _writeBranchService.RemoveAsync(id);
			return Ok();
		}
	}
}