using AutoMapper;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.UserDtos.ReadDtos;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Abstract.UserServices;

namespace Services.Concrete.UserServices;

public class ReadUserService : IReadUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReadUserService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultWithPagingDataDto<List<UserListDto>>> GetUsersListService(UserQuery query)
    {
        ResultWithPagingDataDto<List<UserListDto>> res = new ResultWithPagingDataDto<List<UserListDto>>(query.sayfa,query.search);
        try
        {
            var allData = await Task.Run(() =>
                _unitOfWork.ReadUserRepository.GetAll(
                    predicate: a =>
                                    (string.IsNullOrEmpty(query.search) || a.Username.Contains(query.search))&&
                                    a.Role != UserRoleEnum.SuperAdmin &&
                                    (query.isActive == null ? a.Status==EntityStatusEnum.Online || a.Status == EntityStatusEnum.Offline : (query.isActive == "active" ? a.Status == EntityStatusEnum.Online : a.Status == EntityStatusEnum.Offline)),
                    orderBy: p =>
                    					{
                    						IOrderedQueryable<User> orderedPersonal;
                    						if (query.sortName is not null && query.sortBy is not null)
                    						{
                    							orderedPersonal = query.sortName switch
                    							{
                    								"username" => query.sortBy == "asc"
                    									? p.OrderBy(a => a.Username)
                    									: p.OrderByDescending(a => a.Username),
                    								"email" => query.sortBy == "asc"
                    									? p.OrderBy(a => a.Email)
                    									: p.OrderByDescending(a => a.Email),
                    								"isDefaultPassword" => query.sortBy == "asc"
                    									? p.OrderBy(a => a.IsDefaultPassword)
                    									: p.OrderByDescending(a => a.IsDefaultPassword),
								                    "role" => query.sortBy == "asc"
									                    ? p.OrderBy(a => a.Role)
									                    : p.OrderByDescending(a => a.Role),
                    								_ => p.OrderByDescending(a=> a.CreatedAt)
                    							};
                    						}
                    						else
                    						{
                    							orderedPersonal = p.OrderByDescending(a=> a.CreatedAt);
                    						}
                    
                    						return orderedPersonal;
                    					},
                    include:p=>p.Include(a=>a.BranchUsers).ThenInclude(b=>b.Branch)
                ));   
            var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
                .Take(res.PageSize).ToList();
            var mapData = new List<UserListDto>();
            foreach (var user in resultData)
            {
	            var userListDto = _mapper.Map<UserListDto>(user);
	            userListDto.Branches = user.BranchUsers.OrderBy(bu => bu.Branch.Name).Select(bu => new BranchNameDto { ID = bu.Branch.ID, Name = bu.Branch.Name }).ToList();
	            mapData.Add(userListDto);
            }
            res.SetData(mapData);
            res.TotalRecords = allData.Count();
            res.TotalPages = Convert.ToInt32(Math.Ceiling((double)res.TotalRecords / (double)res.PageSize));
			
        }
        catch (Exception ex)
        {
            res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return res;
    }

    public async Task<IResultWithDataDto<List<BranchNameDto>>> GetDirectorUnusedBranches()
    {
	    IResultWithDataDto<List<BranchNameDto>> result = new ResultWithDataDto<List<BranchNameDto>>();
	    try
	    {
		    var allData = _unitOfWork.ReadBranchRepository.GetAll(
			    predicate: p => 
				    p.Status == EntityStatusEnum.Online &&
				    p.BranchUsers.All(bu => bu.User.Role != UserRoleEnum.Director || bu.User.Status != EntityStatusEnum.Online),
			    include: p=>p.Include(bu=>bu.BranchUsers).ThenInclude(u=>u.User ),
			    orderBy:o=>o.OrderBy(a=>a.Name)
			    );
		    var mappedResult = _mapper.Map<List<BranchNameDto>>(allData);
		    result.SetData(mappedResult);

	    }
	    catch (Exception ex)
	    {
		    result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
	    }

	    return result;
    }

    public async Task<IResultWithDataDto<List<BranchNameDto>>> GetBranchManagerUnusedBranches()
    {
	    IResultWithDataDto<List<BranchNameDto>> result = new ResultWithDataDto<List<BranchNameDto>>();
	    try
	    {
		    var allData = _unitOfWork.ReadBranchRepository.GetAll(
			    predicate: p => 
				    p.Status == EntityStatusEnum.Online &&
				    p.BranchUsers.All(bu => bu.User.Role != UserRoleEnum.BranchManager || bu.User.Status != EntityStatusEnum.Online),
			    include: p=>p.Include(bu=>bu.BranchUsers).ThenInclude(u=>u.User),
			    orderBy:o=>o.OrderBy(a=>a.Name)
		    );
		    var mappedResult = _mapper.Map<List<BranchNameDto>>(allData);
		    result.SetData(mappedResult);

	    }
	    catch (Exception ex)
	    {
		    result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
	    }

	    return result;
    }

    public async Task<IResultWithDataDto<ReadUpdateUserDto>> GetUpdateUserService(Guid id)
    {
	    IResultWithDataDto<ReadUpdateUserDto> result = new ResultWithDataDto<ReadUpdateUserDto>();
	    try
	    {
		    var user = await _unitOfWork.ReadUserRepository.GetSingleAsync(predicate: p => p.ID == id,include:p=>p.Include(a=>a.BranchUsers).ThenInclude(b=> b.Branch));
		    if (user is null) return result.SetStatus(false).SetErr("User Not Found").SetMessage("Kullanıcı Bulunamadı");
		    var mappedResult = _mapper.Map<ReadUpdateUserDto>(user);
		    mappedResult.BranchManagerBranches = new();
		    mappedResult.DirectorBranches = new();
		    if (user.Role != UserRoleEnum.HumanResources)
		    {
			    mappedResult.SelectedBranches = new List<BranchNameDto>();
			    mappedResult.SelectedBranches.AddRange(user.BranchUsers.Select(bu => new BranchNameDto
				    { ID = bu.Branch.ID, Name = bu.Branch.Name }));
			    //mappedResult.Branches.AddRange(mappedResult.SelectedBranches);
			    if (user.Role == UserRoleEnum.Director)
				    mappedResult.DirectorBranches.AddRange(mappedResult.SelectedBranches);
			    else mappedResult.BranchManagerBranches.AddRange(mappedResult.SelectedBranches);
		    };
		    var getDirectorSelect = _unitOfWork.ReadBranchRepository.GetAll(
			    predicate: p => 
				    p.Status == EntityStatusEnum.Online &&
				    p.BranchUsers.All(bu => bu.User.Role != UserRoleEnum.Director || bu.User.Status != EntityStatusEnum.Online),
			    include: p=>p.Include(bu=>bu.BranchUsers).ThenInclude(u=>u.User),
			    orderBy:o=>o.OrderBy(a=>a.Name)
		    ).ToList();
		    var getBranchManagerSelect = _unitOfWork.ReadBranchRepository.GetAll(
			    predicate: p => 
				    p.Status == EntityStatusEnum.Online &&
				    p.BranchUsers.All(bu => bu.User.Role != UserRoleEnum.BranchManager || bu.User.Status != EntityStatusEnum.Online),
			    include: p=>p.Include(bu=>bu.BranchUsers).ThenInclude(u=>u.User),
			    orderBy:o=>o.OrderBy(a=>a.Name)
		    ).ToList();
		    mappedResult.DirectorBranches.AddRange(_mapper.Map<List<BranchNameDto>>(getDirectorSelect)); 
		    mappedResult.BranchManagerBranches.AddRange(_mapper.Map<List<BranchNameDto>>(getBranchManagerSelect)); 
		    result.SetData(mappedResult);
	    }
	    catch (Exception ex)
	    {
		    result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
	    }

	    return result;
    }

    public async Task<IResultWithDataDto<ReadUserSignInDto>> SignInService(ReadUserSignInDto dto)
    {
	    IResultWithDataDto<ReadUserSignInDto> result = new ResultWithDataDto<ReadUserSignInDto>();
	    try
	    {
    
		    var user = await _unitOfWork.ReadUserRepository.GetSingleAsync(predicate: d => d.Email == dto.Email,include:a=>a.Include(b=>b.BranchUsers).ThenInclude(c=>c.Branch));             
		    if (user is null ||	 user.Password != "deneme" && user.Password != "superadmin") // _passwordCryptoHelper.DecryptString(user.Password) != dto.Password           
			    return result.SetStatus(false).SetErr("Not Found User").SetMessage("Girmiş olduğunuz e-posta adresine ait hesap bulunamadı! Lütfen bilgilerinizi kontrol ediniz.");
		    if (user.Status == EntityStatusEnum.Offline) return result.SetStatus(false).SetErr("The User is Banned").SetMessage("Bu bilgilere sahip üyelik pasif duruma alınmıştır.Bir hata olduğunu düşünüyorsanız yetkili ile iletişime geçiniz.");
		    if((user.Role is UserRoleEnum.BranchManager or  UserRoleEnum.Director) && !user.BranchUsers.Any(a => a.Branch.Status == EntityStatusEnum.Online)) return result.SetStatus(false).SetErr("The User Branches Offline").SetMessage("Bu üyeliğe ait şubeler kapatılmış veya pasife alınmış olabilir.Lütfen yetkili ile iletişime geçiniz.");
		    var userMap = _mapper.Map<ReadUserSignInDto>(user);
		    result.SetData(userMap);
	    }
	    catch (Exception ex)
	    {
		    result.SetStatus(false).SetErr(ex.Message).SetMessage("Anlık bir sunucu hatası meydana geldi. Lütfen kısa bir süre sonra tekrar deneyin.");
	    }
	    return result;
    }

    public async Task<IResultWithDataDto<List<Guid>>> GetUserBranches(Guid id)
    {
	    IResultWithDataDto<List<Guid>> result = new ResultWithDataDto<List<Guid>>();
	    try
	    {
		    var branches = _unitOfWork.ReadBranchUserRepository.GetAll(predicate: p => p.UserID == id && p.Branch.Status == EntityStatusEnum.Online,include:i=>i.Include(b=>b.Branch));
		    if(branches.IsNullOrEmpty()) return result.SetStatus(false).SetErr("Not Found Branch").SetMessage("Bir Hata Meydana Geldi");
		    result.SetData(branches.Select(p => p.BranchID).ToList());
	    }
	    catch (Exception ex)
	    {
		    result.SetStatus(false).SetErr(ex.Message).SetMessage("Anlık bir sunucu hatası meydana geldi. Lütfen kısa bir süre sonra tekrar deneyin.");
	    }

	    return result;
    }
}