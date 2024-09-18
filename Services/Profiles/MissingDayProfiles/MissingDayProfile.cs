using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs.DailyCounterDto;
using Core.DTOs.MissingDayDtos.ReadDtos;
using Core.Entities;

namespace Services.Profiles.MissingDayProfiles
{
	public class MissingDayProfile : Profile
	{
		public MissingDayProfile()
		{
			CreateMap<MissingDay, ReadMissingDayDto>()
				.ForMember(dest => dest.NameSurname, opt => opt.MapFrom(src => src.Personal.NameSurname))
				.ForMember(dest => dest.IdentificationNumber, opt => opt.MapFrom(src => src.Personal.IdentificationNumber))
				.ForMember(dest => dest.PersonalStatus, opt => opt.MapFrom(src => src.Personal.Status))
				.ForMember(dest => dest.Personal_Id, opt => opt.MapFrom(src => src.Personal.ID))
				.ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name));
			;
		}
	}
}
