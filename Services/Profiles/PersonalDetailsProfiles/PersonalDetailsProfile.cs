using AutoMapper;
using Core.DTOs.PersonalDetailDto.ReadDtos;
using Core.DTOs.PersonalDetailDto.WriteDtos;
using Core.Entities;

namespace Services.Profiles.PersonalDetailsProfiles;

public class PersonalDetailsProfile : Profile
{
    public PersonalDetailsProfile()
    {
        CreateMap<AddPersonalDetailDto, PersonalDetails>();
        CreateMap<PersonalDetails, ReadUpdatePersonalDetailsDto>();
        CreateMap<AddRangePersonalDetailDto, PersonalDetails>().ReverseMap();
        CreateMap<WriteUpdatePersonalDetailDto, PersonalDetails>();
    }
}