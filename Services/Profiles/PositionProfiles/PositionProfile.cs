using AutoMapper;
using Core.DTOs.PositionDTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Profiles.PositionProfiles
{
	public class PositionProfile : Profile
	{
		public PositionProfile() 
		{ 
			CreateMap<Position,ReadPositionDto>();
			CreateMap<WritePositionDto, Position>();
		}
	}
}
