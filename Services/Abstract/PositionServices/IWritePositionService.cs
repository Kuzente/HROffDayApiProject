using Core.DTOs.BranchDTOs;
using Core.DTOs.PositionDTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract.PositionServices
{
	public interface IWritePositionService
	{
		Task<IResultWithDataDto<ReadPositionDto>> AddAsync(WritePositionDto writePersonalDto);
		Task<IResultWithDataDto<ReadPositionDto>> UpdateAsync(WritePositionDto writePersonalDto);
		Task<IResultWithDataDto<bool>> DeleteAsync(int Id);
		Task<IResultWithDataDto<bool>> RemoveAsync(int Id);
	}
}
