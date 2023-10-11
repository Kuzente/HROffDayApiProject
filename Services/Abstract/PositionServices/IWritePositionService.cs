using Core.DTOs.BaseDTOs;
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
	public interface IWritePositionService : IWriteService<ReadPositionDto , WritePositionDto>
	{
		
	}
}
