using AutoMapper;
using Core;
using Core.DTOs.UserDtos.WriteDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.UserServices;

namespace Services.Concrete.UserServices;

public class WriteUserService : IWriteUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WriteUserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResultDto> AddUserService(AddUserDto dto,Guid userId,string ipAddress)
    {
        IResultDto result = new ResultDto();
        try
        {
            var isUserExist = await _unitOfWork.ReadUserRepository.GetAny(predicate: p => p.Email == dto.Email || p.Username == dto.Username);
            if(isUserExist) return result.SetStatus(false).SetErr("User Already Have").SetMessage("Girmiş olduğunuz kullanıcıya ait bir kayıt mevcut.Lütfen girmiş olduğunuz kullanıcı adı veya mail adresini kontrol ediniz.");
            
            var mappedResult = _mapper.Map<User>(dto);
            if (mappedResult.Role == UserRoleEnum.BranchManager)
            {
                var dtoBranchId = dto.BranchNames.FirstOrDefault();
                var usersBranch = await _unitOfWork.ReadUserRepository.GetSingleAsync(
                        predicate: p =>
                            p.Role == UserRoleEnum.BranchManager &&
                            p.Status == EntityStatusEnum.Online &&
                            p.BranchUsers.Any(a => a.BranchID == dtoBranchId),
                        include:p=>p.Include(a=>a.BranchUsers));
                if(usersBranch is not null) return result.SetStatus(false).SetErr("Branch is already assigned").SetMessage("Girmiş olduğunuz şubeye ait bir şube sorumlusu zaten mevcut lütfen başka bir şube ile deneyiniz.");
            }
            else if (mappedResult.Role == UserRoleEnum.Director)
            {
                var usersBranch = await _unitOfWork.ReadUserRepository.GetSingleAsync(
                    predicate: p =>
                        p.Role == UserRoleEnum.Director &&
                        p.Status == EntityStatusEnum.Online &&
                        p.BranchUsers.Any(a => dto.BranchNames.Contains(a.BranchID)),
                    include:p=>p.Include(a=>a.BranchUsers));
                if(usersBranch is not null) return result.SetStatus(false).SetErr("Branch is already assigned").SetMessage("Girmiş olduğunuz şubeler içerisinde atanmış bir genel müdür zaten mevcut lütfen girdiğiniz şubeleri kontrol ediniz.");
            }
            // Bu kullanıcının şubelerini BranchUser tablosuna ekleyin
            mappedResult.Password = "deneme";
            await _unitOfWork.WriteUserRepository.AddAsync(mappedResult);
            if (dto.BranchNames != null && dto.BranchNames.Any())
            {
                foreach (var branchId in dto.BranchNames)
                {
                    var branchUser = new BranchUser { UserID = mappedResult.ID, BranchID = branchId };
                    await _unitOfWork.WriteBranchUserRepository.AddAsync(branchUser);
                }
            }
            var user = await _unitOfWork.ReadUserRepository.GetSingleAsync(predicate: p => p.ID == userId);
            if(user is null) return result.SetStatus(false).SetErr("User Not Found").SetMessage("Oturumunuz ile ilgili bir problem olabilir. Lütfen Sisteme tekrar giriş yapınız!");
            await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
            {
                EntityName = "User",
                LogType = LogType.Add,
                Description = $"{user.Username} tarafından {mappedResult.Username} adlı Kullanıcı sisteme Eklendi.",
                IpAddress = ipAddress,
                UserID = user.ID,
            });
            var resultCommit = _unitOfWork.Commit();
            if (!resultCommit) return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");

        }
        catch (Exception ex)
        {
            result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return result;
    }

    public async Task<IResultDto> UpdateUserService(WriteUpdateUserDto dto,Guid userId,string ipAddress)
    {
        IResultDto result = new ResultDto();
        try
        {
            var user = await _unitOfWork.ReadUserRepository.GetSingleAsync(
                predicate: p =>
                    p.ID == dto.ID,
                include:p=>p.Include(a=>a.BranchUsers)
            );
            if(user is null) return result.SetStatus(false).SetErr("User is not found").SetMessage("Kullanıcı bulunamadı.");
            user.Username = dto.Username;
            user.Status = dto.Status;
            user.Role = dto.Role;
            var newBranchUserList = new List<BranchUser>();
            if (dto.Role == UserRoleEnum.BranchManager)
            {
                var getbranchUser = await _unitOfWork.ReadBranchUserRepository.GetAny(predicate: p =>
                    dto.BranchNames.Contains(p.BranchID) && p.User.Role == UserRoleEnum.BranchManager && p.UserID != dto.ID && p.User.Status == EntityStatusEnum.Online);
                if(getbranchUser) return result.SetStatus(false).SetErr("Branch already have").SetMessage("Girmek İstediğiniz Şubeye ait kayıt mevcut");
            }
            if (dto.Role == UserRoleEnum.Director)
            {
                var getbranchUser = await _unitOfWork.ReadBranchUserRepository.GetAny(predicate: p =>
                    dto.BranchNames.Contains(p.BranchID) && p.User.Role == UserRoleEnum.Director && p.UserID != dto.ID && p.User.Status == EntityStatusEnum.Online);
                if(getbranchUser) return result.SetStatus(false).SetErr("Branch already have").SetMessage("Girmek İstediğiniz Şubeye ait kayıt mevcut");
            }
            if (user.BranchUsers.Any())
            {
                await _unitOfWork.WriteBranchUserRepository.RemoveRangeAsync(user.BranchUsers.ToList());

            }
            if (dto.BranchNames != null)
            {
                dto.BranchNames.ForEach(a =>
                {
                    var branchUser = new BranchUser
                    {
                        UserID = user.ID,
                        BranchID = a,
                    };
                    newBranchUserList.Add(branchUser);
                });
            }
            user.BranchUsers = newBranchUserList;
            
            await _unitOfWork.WriteUserRepository.Update(user);
            var clientUser = await _unitOfWork.ReadUserRepository.GetSingleAsync(predicate: p => p.ID == userId);
            if(clientUser is null) return result.SetStatus(false).SetErr("User Not Found").SetMessage("Oturumunuz ile ilgili bir problem olabilir. Lütfen Sisteme tekrar giriş yapınız!");
            await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
            {
                EntityName = "User",
                LogType = LogType.Update,
                Description = $"{clientUser.Username} tarafından {user.Username} adlı kullanıcı Güncellendi.",
                IpAddress = ipAddress,
                UserID = clientUser.ID,
            });
            var resultCommit = _unitOfWork.Commit();
            if (!resultCommit) return result.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
        }
        catch (Exception ex)
        {
            result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return result;
    }

    public async Task<IResultDto> DeleteUserService(Guid id,Guid userId,string ipAddress)
    {
        IResultDto res = new ResultDto();
        try
        {
            var getResult = await _unitOfWork.ReadUserRepository.GetByIdAsync(id);
            if(getResult.FirstOrDefault() is null) return res.SetStatus(false).SetErr("User Not Found").SetMessage("Kullanıcı Bulunamadı");
            var result = await _unitOfWork.WriteUserRepository.RemoveByIdAsync(id);
            if (!result) return res.SetStatus(false).SetErr("Data Layer Error").SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
            var user = await _unitOfWork.ReadUserRepository.GetSingleAsync(predicate: p => p.ID == userId);
            if(user is null) return res.SetStatus(false).SetErr("User Not Found").SetMessage("Oturumunuz ile ilgili bir problem olabilir. Lütfen Sisteme tekrar giriş yapınız!");
            await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
            {
                EntityName = "User",
                LogType = LogType.Delete,
                Description = $"{user.Username} tarafından {getResult.FirstOrDefault().Username} adlı Kullanıcı sistemden silindi.",
                IpAddress = ipAddress,
                UserID = user.ID,
            });
            var resultCommit = _unitOfWork.Commit();
            if (!resultCommit) return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
        }
        catch (Exception ex)
        {
            res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }
        return res;
    }
}