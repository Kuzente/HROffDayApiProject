using AutoMapper;
using Core.DTOs.PersonalDTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Profiles.PersonalProfiles
{
	public class PersonalProfile : Profile
	{
		public PersonalProfile() 
		{
			CreateMap<Personal,ReadPersonalDto>();
			CreateMap<WritePersonalDto,Personal>();
		}
	}
}
