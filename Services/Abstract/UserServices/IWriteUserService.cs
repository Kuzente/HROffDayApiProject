using Core.DTOs.UserDtos.WriteDtos;
using Core.Interfaces;

namespace Services.Abstract.UserServices;

public interface IWriteUserService
{
    Task<IResultDto> AddUserService(AddUserDto dto,Guid userId,string ipAddress);
    Task<IResultDto> UpdateUserService(WriteUpdateUserDto dto,Guid userId,string ipAddress);
    Task<IResultDto> DeleteUserService(Guid id,Guid userId,string ipAddress);
    
}