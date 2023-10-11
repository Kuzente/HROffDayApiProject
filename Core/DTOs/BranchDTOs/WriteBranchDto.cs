using Core.DTOs.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.BranchDTOs
{
	public class WriteBranchDto : WriteBaseDto
	{
		public string Name { get; set; }
	}
}
