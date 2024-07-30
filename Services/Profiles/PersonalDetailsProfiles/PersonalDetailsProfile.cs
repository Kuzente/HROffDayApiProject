using AutoMapper;
using Core.DTOs.OffDayDTOs.ReadDtos;
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

        #region Personel İzinleri

        CreateMap<OffDay, ReadPersonalOffDayListDto>();
        CreateMap<Personal, ReadPersonalOffDayListDtoSubPersonal>();
        CreateMap<Personal, ReadPersonalDetailsHeaderDto>();
        CreateMap<Branch, ReadPersonalDetailsHeaderSubBranchDto>();
        CreateMap<Position, ReadPersonalDetailsHeaderSubPositionDto>();
        

        #endregion
    }
}