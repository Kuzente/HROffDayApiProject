using Core.DTOs.BranchDTOs;
using Core.DTOs.PersonalDTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract.BranchServices
{
	public interface IWriteBranchService : IWriteService<ReadBranchDto,WriteBranchDto>
	{
		
	}
}
