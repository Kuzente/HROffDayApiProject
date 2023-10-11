using AutoMapper;
using Core.DTOs.OffDayDTOs;
using Core.DTOs.PositionDTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Profiles.OffdayProfiles
{
	public class OffDayProfile : Profile
	{
		public OffDayProfile() 
		{
			CreateMap<OffDay, ReadOffDayDto>();
			CreateMap<WriteOffDayDto, OffDay>();
		}
	}
}
