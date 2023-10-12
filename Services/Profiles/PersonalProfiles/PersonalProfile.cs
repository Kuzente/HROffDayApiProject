using AutoMapper;
using Core.DTOs.PersonalDTOs;
using Core.Entities;

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
