using Core.DTOs.PersonalDTOs;
using Core.DTOs.PositionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract.PositionServices
{
	public interface IReadPositionService
	{
		Task<List<ReadPositionDto>> GetAllAsync();
		Task<ReadPositionDto> GetSingleAsync();
		Task<bool> GetAnyAsync();
	}
}
