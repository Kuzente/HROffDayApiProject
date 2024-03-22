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
		#region Bekleyen İzinler

		CreateMap<OffDay, ReadWaitingOffDayListDto>();
		CreateMap<Personal, ReadWaitingOffDayListPersonalDto>();

		CreateMap<OffDay, ReadWaitingOffDayEditDto>();
		CreateMap<Personal, ReadWaitingOffDayEditSubPersonalDto>();
		
		CreateMap<WriteUpdateWatingOffDayDto, OffDay>();
		#endregion

		#region Reddedilen İzinler
		CreateMap<OffDay, ReadRejectedOffDayListDto>();
		CreateMap<Personal, ReadRejectedOffDayListPersonelDto>();
		
		#endregion
		

		#region Onaylanan İzinler
		CreateMap<OffDay, ReadApprovedOffDayListDto>();
		CreateMap<Personal, ReadApprovedOffDayListSubPersonalDto>();
		// CreateMap<Branch, ReadApprovedOffDayListSubPersonalBranchDto>();
		// CreateMap<Position, ReadApprovedOffDayListSubPersonalPositionDto>();


		CreateMap<OffDay, ReadApprovedOffDayFormExcelExportDto>();
		CreateMap<Personal, ReadApprovedOffDayFormExcelExportSubPersonalDto>();
		CreateMap<Branch, ReadApprovedOffDayFormExcelExportSubPersonalSubBranchDto>();
		CreateMap<Position, ReadApprovedOffDayFormExcelExportSubPersonalSubPositionDto>();
		#endregion

		#region Silinen İzinler
		CreateMap<OffDay, ReadDeletedOffDayListDto>();
		CreateMap<Personal, ReadDeletedOffDayListSubPersonalDto>();
		

		#endregion
		
		CreateMap<WriteAddOffDayDto, OffDay>();
		
	}
}