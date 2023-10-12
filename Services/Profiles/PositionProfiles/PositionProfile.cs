using AutoMapper;
using Core.DTOs.PositionDTOs;
using Core.Entities;

namespace Services.Profiles.PositionProfiles;

public class PositionProfile : Profile
{
	public PositionProfile() 
	{ 
		CreateMap<Position,ReadPositionDto>();
		CreateMap<WritePositionDto, Position>();
	}
}