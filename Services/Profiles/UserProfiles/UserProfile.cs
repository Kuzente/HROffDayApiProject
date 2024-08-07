using AutoMapper;
using Core.DTOs.UserDtos.ReadDtos;
using Core.DTOs.UserDtos.WriteDtos;
using Core.Entities;

namespace Services.Profiles.UserProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AddUserDto, User>();
        CreateMap<User, UserListDto>();
        CreateMap<User, ReadUserSignInDto>();
        CreateMap<User, ReadUpdateUserDto>();
        CreateMap<User, ReadUserDto>();
    }
}