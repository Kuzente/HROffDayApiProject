using Core.DTOs.BranchDTOs;
using Core.DTOs.PersonalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract.BranchServices
{
	public interface IReadBranchService
	{
		Task<List<ReadBranchDto>> GetAllAsync();
		Task<ReadBranchDto> GetSingleAsync();
		Task<bool> GetAnyAsync();
	}
}
