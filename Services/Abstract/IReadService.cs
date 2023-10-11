using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
	public interface IReadService<T> : IService<T> where T : BaseDto
	{
		Task<List<T>> GetAllAsync();
		Task<T> GetSingleAsync();
		Task<bool> GetAnyAsync();
	}
}
