using Core.DTOs.UserDtos.WriteDtos;
using Core.Interfaces;

namespace Services.Abstract.UserServices;

public interface IWriteUserService
{
    Task<IResultDto> AddUserService(AddUserDto dto);
    Task<IResultDto> UpdateUserService(WriteUpdateUserDto dto);
    Task<IResultDto> DeleteUserService(Guid id);
    
}