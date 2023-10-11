using Core.DTOs.PersonalDTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract.PersonalServices
{
	public interface IWritePersonalService 
	{
		Task<IResultWithDataDto<ReadPersonalDto>> AddAsync(WritePersonalDto writePersonalDto);
		Task<IResultWithDataDto<ReadPersonalDto>> UpdateAsync(WritePersonalDto writePersonalDto);
		Task<IResultWithDataDto<bool>> DeleteAsync(int Id);
		Task<IResultWithDataDto<bool>> RemoveAsync(int Id);
	}
}
