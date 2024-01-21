﻿using AutoMapper;
using Core.DTOs.OffDayDTOs;
using Core.DTOs.OffDayDTOs.ReadDtos;
using Core.DTOs.OffDayDTOs.WriteDtos;
using Core.Entities;

namespace Services.Profiles.OffDayProfiles;

public class OffDayProfile : Profile
{
	public OffDayProfile() 
	{
		#region Bekleyen İzinler

		CreateMap<OffDay, ReadWaitingOffDayListDto>();

		
		CreateMap<Personal, ReadWaitingOffDayListPersonalDto>();
		CreateMap<Branch, ReadWaitingOffDayListPersonalBranchDto>();
		CreateMap<OffDay, ReadWaitingOffDayEditDto>();
		CreateMap<Personal, ReadWaitingOffDayEditSubPersonalDto>();
		CreateMap<Branch, ReadWaitingOffDayEditSubPersonalSubBranchDto>();
		CreateMap<Position, ReadWaitingOffDayEditSubPersonalSubPositionDto>();
		
		CreateMap<WriteUpdateWatingOffDayDto, OffDay>();
		#endregion

		#region Reddedilen İzinler
		CreateMap<OffDay, ReadRejectedOffDayListDto>();
		CreateMap<Personal, ReadRejectedOffDayListPersonelDto>();
		CreateMap<Branch, ReadRejectedOffDayListPersonelBranchDto>();
		

		#endregion
		

		#region Onaylanan İzinler
		CreateMap<OffDay, ReadApprovedOffDayListDto>();
		CreateMap<Personal, ReadApprovedOffDayListSubPersonalDto>();
		CreateMap<Branch, ReadApprovedOffDayListSubPersonalBranchDto>();
		CreateMap<Position, ReadApprovedOffDayListSubPersonalPositionDto>();

		#endregion

		#region Silinen İzinler
		CreateMap<OffDay, ReadDeletedOffDayListDto>();
		CreateMap<Personal, ReadDeletedOffDayListSubPersonalDto>();
		CreateMap<Branch, ReadDeletedOffDayListSubPersonalBranchDto>();
		CreateMap<Position, ReadDeletedOffDayListSubPersonalPositionDto>();
		

		#endregion
		
		CreateMap<WriteAddOffDayDto, OffDay>();
		
	}
}