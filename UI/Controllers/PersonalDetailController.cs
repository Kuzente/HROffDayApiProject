using Core;
using Core.DTOs.PersonalDTOs.WriteDtos;
using Core.Interfaces;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.OffDayServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;

namespace UI.Controllers;

[Authorize]
public class PersonalDetailController : Controller
{
    private readonly IReadPersonalService _readPersonalService;
    private readonly IReadOffDayService _readOffDayService;
    private readonly IWritePersonalService _writePersonalService;

    public PersonalDetailController(IReadPersonalService readPersonalService, IWritePersonalService writePersonalService,IReadOffDayService readOffDayService)
    {
        _readPersonalService = readPersonalService;
        _writePersonalService = writePersonalService;
        _readOffDayService = readOffDayService;
    }

    #region PageActions
    /// <summary>
    /// Personel Detayları Sayfası
    /// </summary>
    /// <returns></returns>
    public IActionResult Edit(Guid id)
    {
        return View();
    }
    /// <summary>
    /// Personel İzinleri Sayfası
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> PersonalOffDayList(OffdayQuery query)
    {
        var result = await _readOffDayService.GetPersonalOffDaysListService(query);
        return View(result);
    }

    #endregion

    #region Get/Post Actions
    /// <summary>
    /// Personel Detayları Ajax Get Metodu
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> EditAjax(Guid id)
    {
        var result = await _readPersonalService.GetUpdatePersonalService(id);
        return Ok(result);
    }
    
    /// <summary>
    /// Personel Detayları Güncelleme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Edit(WriteUpdatePersonalDto dto)
    {
        IResultDto result = new ResultDto();
        if (!ModelState.IsValid)
        {
            result.SetStatus(false).SetErr("ModelState is not Valid").SetMessage("Lütfen Zorunlu Alanları Doğru Girdiğinize Emin Olunuz");
        }
        else
        {
            result = await _writePersonalService.UpdateAsync(dto);
        }
        
        return Ok(result);
    }
    /// <summary>
    /// Personel Sil Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ArchivePersonal(Guid id)
    {
        var result = await _writePersonalService.DeleteAsync(id);
        return Ok(result);
    }
    /// <summary>
    /// Personel İşten Çıkar veya İşe Al Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ChangeStatus(Guid id)
    {
        var result = await _writePersonalService.ChangeStatus(id);
        return Ok(result);
    }
    /// <summary>
    /// Personel İzinleri Personel Header Ajax Get Metodu
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> PersonelDetailsHeader(Guid id)
    {
        var result = await _readPersonalService.GetPersonalDetailsHeaderByIdService(id);
        return Ok(result);
    }
    #endregion
  
   
   
}