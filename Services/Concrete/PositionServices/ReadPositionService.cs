using Core.DTOs.PositionDTOs;
using Services.Abstract.PositionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete.PositionServices
{
	public class ReadPositionService : IReadPositionService
	{
		public Task<List<ReadPositionDto>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<bool> GetAnyAsync()
		{
			throw new NotImplementedException();
		}

		public Task<ReadPositionDto> GetSingleAsync()
		{
			throw new NotImplementedException();
		}
	}
}
