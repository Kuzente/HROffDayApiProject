using Core;
using Core.DTOs.UserDtos.WriteDtos;
using Core.Enums;
using Core.Interfaces;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.UserServices;

namespace UI.Controllers;
[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
public class UserController : Controller
{
    private readonly IWriteUserService _writeUserService;
    private readonly IReadUserService _readUserService;

    public UserController(IWriteUserService writeUserService, IReadUserService readUserService)
    {
        _writeUserService = writeUserService;
        _readUserService = readUserService;
    }

    // GET
    public async Task<IActionResult> UsersList(UserQuery query)
    {
        var result = await _readUserService.GetUsersListService(query);
        return View(result);
    }
    public async Task<IActionResult> GetDirectorSelects()
    {
        var result = await _readUserService.GetDirectorUnusedBranches();
        return Ok(result);
    }
    public async Task<IActionResult> GetBranchManagerSelects()
    {
        var result = await _readUserService.GetBranchManagerUnusedBranches();
        return Ok(result);
    }
    public async Task<IActionResult> UpdateUser(Guid id)
    {
        var result = await _readUserService.GetUpdateUserService(id);
        return View(result);
    }
    /// <summary>
    /// Kullanıcı Ekleme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateUser(AddUserDto dto)
    {
        IResultDto result = new ResultDto();
        if (!ModelState.IsValid) return Ok(result.SetStatus(false).SetErr("ModelState is not valid").SetMessage("Lütfen Tüm Alanları Girdiğinizden Emin Olunuz."));
        if(dto.Role is UserRoleEnum.Director or UserRoleEnum.BranchManager && dto.BranchNames is null) return Ok(result.SetStatus(false).SetErr("ModelState is not valid").SetMessage("Lütfen Tüm Alanları Girdiğinizden Emin Olunuz."));
        result = await _writeUserService.AddUserService(dto);
        
        return Ok(result);
    }
    /// <summary>
    /// Kullanıcı Güncelleme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> UpdateUser(WriteUpdateUserDto dto)
    {
        IResultDto result = new ResultDto();
        if (!ModelState.IsValid) return Ok(result.SetStatus(false).SetErr("ModelState is not valid").SetMessage("Lütfen Tüm Alanları Girdiğinizden Emin Olunuz."));
        if(dto.Role is UserRoleEnum.Director or UserRoleEnum.BranchManager && dto.BranchNames is null) return Ok(result.SetStatus(false).SetErr("ModelState is not valid").SetMessage("Lütfen Tüm Alanları Girdiğinizden Emin Olunuz."));
        result = await _writeUserService.UpdateUserService(dto);
        
        return Ok(result);
    }
    /// <summary>
    /// Kullanıcı Silme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> DeleteUser(Guid id, string returnUrl)
    {
        var result = await _writeUserService.DeleteUserService(id);
        return Ok(result);
    }
}