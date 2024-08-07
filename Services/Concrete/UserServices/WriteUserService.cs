using AutoMapper;
using Core;
using Core.DTOs;
using Core.DTOs.AuthenticationDtos.ReadDtos;
using Core.DTOs.UserDtos.WriteDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.UserServices;
using Services.HelperServices;
using System.Net;
using System.Web;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace Services.Concrete.UserServices;

public class WriteUserService : IWriteUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PasswordCryptoHelper _passwordCryptoHelper;
    private readonly MailHelper _mailHelper;

	public WriteUserService(IUnitOfWork unitOfWork, IMapper mapper, PasswordCryptoHelper passwordCryptoHelper, MailHelper mailHelper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_passwordCryptoHelper = passwordCryptoHelper;
		_mailHelper = mailHelper;
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
            mappedResult.Password = _passwordCryptoHelper.GenerateEmailToken(mappedResult.Username);
            mappedResult.MailVerificationToken = _passwordCryptoHelper.GenerateEmailToken(mappedResult.Email);
            mappedResult.TokenExpiredDate = DateTime.Now.AddDays(3);
            mappedResult.IsDefaultPassword = true;
            await _unitOfWork.WriteUserRepository.AddAsync(mappedResult);
            if (dto.BranchNames != null && dto.BranchNames.Any())
            {
                foreach (var branchId in dto.BranchNames)
                {
                    var branchUser = new BranchUser { UserID = mappedResult.ID, BranchID = branchId };
                    await _unitOfWork.WriteBranchUserRepository.AddAsync(branchUser);
                }
            }
            await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
            {
                EntityName = "User",
                LogType = LogType.Add,
                Description = $"{mappedResult.Username} adlı Kullanıcı sisteme Eklendi.",
                IpAddress = ipAddress,
                UserID = userId,
            });
            var mailHtml = _mailHelper.GetMailemplateHtml(
                "activation",
                mappedResult.Username, 
                HttpUtility.UrlEncode(_passwordCryptoHelper.EncryptString(mappedResult.MailVerificationToken)),
                HttpUtility.UrlEncode(_passwordCryptoHelper.EncryptString(mappedResult.ID.ToString())),
				HttpUtility.UrlEncode(_passwordCryptoHelper.EncryptString(mappedResult.Email)), 
                mappedResult.TokenExpiredDate);
            var isMailSendSuccess = await _mailHelper.SendEmail(mappedResult.Email, "İyaş Personel Takip Hesap Aktivasyonu", mailHtml);
            if(isMailSendSuccess is false || string.IsNullOrEmpty(mailHtml))
				return result.SetStatus(false).SetErr("Mail Send Fail").SetMessage("Eposta Gönderilirken Bir Hata oluştu! Lütfen daha sonra tekrar deneyiniz...");

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
            await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
            {
                EntityName = "User",
                LogType = LogType.Update,
                Description = $"{user.Username} adlı kullanıcı Güncellendi.",
                IpAddress = ipAddress,
                UserID = userId,
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
            await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
            {
                EntityName = "User",
                LogType = LogType.Delete,
                Description = $"{getResult.FirstOrDefault().Username} adlı Kullanıcı sistemden silindi.",
                IpAddress = ipAddress,
                UserID = userId,
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

	public async Task<IResultWithDataDto<ConfirmForgotPassDto>> ForgotPasswordConfirmEmailService(ConfirmForgotPassDto dto)
	{
		IResultWithDataDto<ConfirmForgotPassDto> res = new ResultWithDataDto<ConfirmForgotPassDto>();
		try
		{
			var decryptToken = _passwordCryptoHelper.DecryptString(dto.Token);
			var decryptMail = _passwordCryptoHelper.DecryptString(dto.mail);
			var decryptUserId = _passwordCryptoHelper.DecryptString(dto.UserId);
			var forgotPassData = await _unitOfWork.ReadUserRepository.GetSingleAsync(
                predicate: p =>
				p.MailVerificationToken == decryptToken &&
				p.Email == decryptMail &&
				p.ID.ToString() == decryptUserId
				);
			if (forgotPassData is null) return res.SetStatus(false).SetErr("Invalid Request Parameters").SetMessage("Bir Sorun Oluştu. Lütfen Şifrenizi Tekrar Yenileyiniz!!");
			if (!forgotPassData.IsDefaultPassword) return res.SetStatus(false).SetErr("ForgotPass Is Already Used").SetMessage("Bu bağlantı ile şifrenizi zaten sıfırlamışsınız!!!");
			if (forgotPassData.TokenExpiredDate < DateTime.Now)
				return res.SetStatus(false).SetErr("Token Date Expired").SetMessage(
					"Şifre sıfırlama bağlantınızın süresi dolmuş.Lütfen şifremi unuttum kısmından yeni bir bağlantı ile deneyiniz!!!");
			res.SetData(dto);
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}

	public async Task<IResultDto> ResetPasswordService(ResetPasswordDto dto,string ipAddress)
	{
		IResultDto res = new ResultDto();
		try
		{
			var decryptUserId = _passwordCryptoHelper.DecryptString(dto.UserId);
			var decryptMail = _passwordCryptoHelper.DecryptString(dto.Mail);
			var decryptToken = _passwordCryptoHelper.DecryptString(dto.Token);
			var user = await _unitOfWork.ReadUserRepository.GetSingleAsync(predicate: p => p.ID.ToString() == decryptUserId);
			if (user is null) return res.SetStatus(false).SetErr("User Not Found").SetMessage("İlgili Kullanıcı Bulunamadı!!!");
            if (user.Status == EntityStatusEnum.Offline) return res.SetStatus(false).SetErr("The User is Passive").SetMessage("Bu bilgilere sahip üyelik pasif haldedir.Bir hata olduğunu düşünüyorsanız yetkili ile iletişime geçiniz.");
            if(user.IsDefaultPassword == false)
				return res.SetStatus(false).SetErr("User already reset password").SetMessage("Şifrenizi zaten sıfırlamışsınız!!!");
			user.Password = _passwordCryptoHelper.EncryptString(dto.Password);
            user.IsDefaultPassword = false;
            user.TokenExpiredDate = DateTime.MinValue;
            user.MailVerificationToken = "-";
			await _unitOfWork.WriteUserRepository.Update(user);
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "User",
				LogType = LogType.ResetPass,
				Description = $"{user.Username} adlı Kullanıcı şifresini sıfırladı.",
				IpAddress = ipAddress,
				UserID = user.ID,
			});
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");

		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}

	public async Task<IResultDto> ForgotPasswordService(ForgotPasswordPostDto dto, string ipAddress)
	{
		IResultDto res = new ResultDto();
		try
		{
			var user = await _unitOfWork.ReadUserRepository.GetSingleAsync(predicate: p => p.Email == dto.Email && p.Status == EntityStatusEnum.Online);
			if (user is null) return res.SetStatus(false).SetErr("User Not Found").SetMessage("İlgili Kullanıcı Bulunamadı!!!");
			if (user.TokenExpiredDate > DateTime.Now)
				return res.SetStatus(false).SetErr("There is already open reset link").SetMessage("Mail adresinize sıfırlama bağlantısı zaten gönderilmiş!");
			user.IsDefaultPassword = true;
            user.Password = _passwordCryptoHelper.EncryptString(_passwordCryptoHelper.GenerateEmailToken(user.Email));
			user.TokenExpiredDate = DateTime.Now.AddDays(3);
			user.MailVerificationToken = _passwordCryptoHelper.GenerateEmailToken(user.Email);
			await _unitOfWork.WriteUserRepository.Update(user);
			await _unitOfWork.WriteUserLogRepository.AddAsync(new UserLog
			{
				EntityName = "User",
				LogType = LogType.ForgotPassMail,
				Description = $"{user.Username} adlı kullanıcıya şifre sıfırlama bağlantısı gönderildi.",
				IpAddress = ipAddress,
				UserID = user.ID,
			});
			var mailHtml = _mailHelper.GetMailemplateHtml(
				"forgotpassword",
				user.Username,
				HttpUtility.UrlEncode(_passwordCryptoHelper.EncryptString(user.MailVerificationToken)),
				HttpUtility.UrlEncode(_passwordCryptoHelper.EncryptString(user.ID.ToString())),
				HttpUtility.UrlEncode(_passwordCryptoHelper.EncryptString(user.Email)),
				user.TokenExpiredDate);
			var isMailSendSuccess = await _mailHelper.SendEmail(user.Email, "İyaş Personel Takip Şifre Sıfırla", mailHtml);
			if (isMailSendSuccess is false || string.IsNullOrEmpty(mailHtml))
				return res.SetStatus(false).SetErr("Mail Send Fail").SetMessage("Eposta Gönderilirken Bir Hata oluştu! Lütfen daha sonra tekrar deneyiniz...");
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");

		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
	}
}