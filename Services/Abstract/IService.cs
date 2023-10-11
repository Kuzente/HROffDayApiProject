using Core.DTOs.BaseDTOs;
using Core.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
	public interface IService<T> where T : BaseDto
	{
	}
}
