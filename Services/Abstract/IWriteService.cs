using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.Interfaces;
using Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
	public interface IWriteService<T,T1> : IService<T> where T : ReadBaseDto where T1 : WriteBaseDto
	{
		Task<IResultWithDataDto<T>> AddAsync(T1 writeDto);
		Task<IResultWithDataDto<T>> UpdateAsync(T1 writeDto);
		Task<bool> DeleteAsync(int Id);
		Task<bool> RemoveAsync(int Id);
	}
}
