using AutoMapper;
using Core.DTOs.OffDayDTOs;
using Core.DTOs.OffDayDTOs.ReadDtos;
using Core.DTOs.OffDayDTOs.WriteDtos;
using Core.Entities;

namespace Services.Profiles.OffDayProfiles;

public class OffDayProfile : Profile
{
	public OffDayProfile() 
	{
		CreateMap<OffDay, ReadWaitingOffDayListDto>();
		CreateMap<OffDay, ReadWaitingOffDayEditDto>();
		CreateMap<Personal, ReadWaitingOffDayEditSubPersonalDto>();
		CreateMap<Branch, ReadWaitingOffDayEditSubPersonalSubBranchDto>();
		CreateMap<Position, ReadWaitingOffDayEditSubPersonalSubPositionDto>();
		CreateMap<WriteAddOffDayDto, OffDay>();
		CreateMap<WriteUpdateWatingOffDayDto, OffDay>();
	}
}