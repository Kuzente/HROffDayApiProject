using AutoMapper;
using Core.DTOs.PassivePersonalDtos;
using Core.DTOs.PersonalDTOs;
using Core.DTOs.PersonalDTOs.ReadDtos;
using Core.DTOs.PersonalDTOs.WriteDtos;
using Core.Entities;

namespace Services.Profiles.PersonalProfiles;

public class PersonalProfile : Profile
{
	public PersonalProfile() 
	{
		CreateMap<Personal,PersonalDto>().ReverseMap();
		CreateMap<Personal,PassivePersonalDto>().ReverseMap();
		CreateMap<Personal,PersonalOffDayDto>().ReverseMap();
		
		CreateMap<Personal,ReadUpdatePersonalDto>();
		CreateMap<AddPersonalDto,Personal>().ReverseMap();
		CreateMap<AddRangePersonalDto,Personal>().ReverseMap();
		CreateMap<PersonalDetailDto,Personal>().ReverseMap();
		
		CreateMap<WritePersonalDto,Personal>();
		CreateMap<WriteUpdatePersonalDto, Personal>();
	}
}