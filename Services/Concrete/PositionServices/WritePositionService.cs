using Core.DTOs.PositionDTOs;
using Core.Interfaces;
using Services.Abstract.PositionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete.PositionServices
{
	public class WritePositionService : IWritePositionService
	{
		public Task<IResultWithDataDto<ReadPositionDto>> AddAsync(WritePositionDto writeBranchDto)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(int Id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> RemoveAsync(int Id)
		{
			throw new NotImplementedException();
		}

		public Task<IResultWithDataDto<ReadPositionDto>> UpdateAsync(WritePositionDto writeBranchDto)
		{
			throw new NotImplementedException();
		}
	}
}
