using AutoMapper;
using Core.DTOs.UserLogDTOs.ReadDtos;
using Core.Entities;

namespace Services.Profiles.UserLogProfiles;

public class UserLogProfile: Profile
{
    public UserLogProfile()
    {
        CreateMap<UserLog, HeaderLastFiveLogDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
		;
		CreateMap<UserLog, UsersLogListDto>()
			.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
		;
	}
}