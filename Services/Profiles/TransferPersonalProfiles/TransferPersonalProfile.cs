using AutoMapper;
using Core.DTOs.TransferPersonalDtos.ReadDtos;
using Core.Entities;

namespace Services.Profiles.TransferPersonalProfiles;

public class TransferPersonalProfile : Profile
{
    public TransferPersonalProfile()
    {
        CreateMap<TransferPersonal, ReadTransferPersonalDto>()
			.ForMember(dest => dest.PersonalNameSurname, opt => opt.MapFrom(src => src.Personal.NameSurname));
		;
    }
}