using AutoMapper;
using Core.DTOs.PositionDTOs;
using Core.Entities;

namespace Services.Profiles.PositionProfiles;

public class PositionProfile : Profile
{
	public PositionProfile() 
	{ 
		CreateMap<Position,PositionDto>().ReverseMap();
		CreateMap<Position,PositionNameDto>().ReverseMap();
		
	}
}