using AutoMapper;
using Core.DTOs.PersonalCumulativeDtos.ReadDtos;
using Core.Entities;

namespace Services.Profiles.PersonalCumulativesProfiles;

public class PersonalCumulativeProfile : Profile
{
    public PersonalCumulativeProfile()
    {
        CreateMap<PersonalCumulative, ReadUpdatePersonalCumulativeDto>().ReverseMap();
    }
}