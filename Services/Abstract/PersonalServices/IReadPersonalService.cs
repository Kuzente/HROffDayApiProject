using Core.DTOs.PersonalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract.PersonalServices
{
	public interface IReadPersonalService : IReadService<ReadPersonalDto>
	{
		Task<List<ReadPersonalDto>> GetAllWithBranchAndPositionAsync();
	}
}
