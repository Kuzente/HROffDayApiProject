using AutoMapper;
using Core.DTOs.BranchDTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Profiles.BranchProfiles
{
	public class BranchProfile : Profile
	{
		public BranchProfile() 
		{
			CreateMap<Branch , ReadBranchDto>();
			CreateMap<WriteBranchDto, Branch>();
		}
	}
}
