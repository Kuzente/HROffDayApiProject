using AutoMapper;
using Core.DTOs.DailyCounterDto;
using Core.Entities;

namespace Services.Profiles.DailyCounterProfiles;

public class DailyCounterProfile : Profile
{
    public DailyCounterProfile()
    {
        CreateMap<DailyCounter, TodayStartPersonalDto>();
    }
}