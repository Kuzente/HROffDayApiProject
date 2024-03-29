using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.UserDtos.ReadDtos;
using Core.Interfaces;
using Core.Querys;

namespace Services.Abstract.UserServices;

public interface IReadUserService
{
    Task<ResultWithPagingDataDto<List<UserListDto>>> GetUsersListService(UserQuery query); // Şube Listesi Servisi
    Task<IResultWithDataDto<List<BranchNameDto>>> GetDirectorUnusedBranches();
    Task<IResultWithDataDto<List<BranchNameDto>>> GetBranchManagerUnusedBranches();
    Task<IResultWithDataDto<ReadUpdateUserDto>> GetUpdateUserService(Guid id); // Şube Güncelleme Get Servisi
    Task<IResultWithDataDto<ReadUserSignInDto>> SignInService(ReadUserSignInDto dto);
    Task<IResultWithDataDto<List<Guid>>> GetUserBranches(Guid id);
}