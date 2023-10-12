using AutoMapper;
using Core.DTOs.OffDayDTOs;
using Core.Entities;

namespace Services.Profiles.OffDayProfiles;

public class OffDayProfile : Profile
{
	public OffDayProfile() 
	{
		CreateMap<OffDay, ReadOffDayDto>();
		CreateMap<WriteOffDayDto, OffDay>();
	}
}