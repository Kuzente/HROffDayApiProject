using AutoMapper;
using Core.DTOs.BranchDTOs;
using Core.Entities;

namespace Services.Profiles.BranchProfiles;

public class BranchProfile : Profile
{
	public BranchProfile() 
	{
		CreateMap<Branch , BranchDto>().ReverseMap();
		CreateMap<Branch , BranchNameDto>().ReverseMap();
		
	}
}