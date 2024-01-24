using Core.DTOs.PersonalDTOs.WriteDtos;
using Core.Querys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Services.Abstract.BranchServices;
using Services.Abstract.OffDayServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;

namespace UI.Controllers;

[Authorize]
public class PersonalDetailController : Controller
{
    private readonly IToastNotification _toastNotification;
    private readonly IReadPositionService _readPositionService;
    private readonly IReadBranchService _readBranchService;
    private readonly IReadPersonalService _readPersonalService;
    private readonly IReadOffDayService _readOffDayService;
    private readonly IWritePersonalService _writePersonalService;

    public PersonalDetailController(IReadBranchService readBranchService, IReadPositionService readPositionService, IReadPersonalService readPersonalService, IWritePersonalService writePersonalService, IToastNotification toastNotification, IReadOffDayService readOffDayService)
    {
        _readBranchService = readBranchService;
        _readPositionService = readPositionService;
        _readPersonalService = readPersonalService;
        _writePersonalService = writePersonalService;
        _toastNotification = toastNotification;
        _readOffDayService = readOffDayService;
    }

    #region PageActions
    /// <summary>
    /// Personel Detayları Sayfası
    /// </summary>
    /// <returns></returns>
    public IActionResult Edit(int id)
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
        if (!result.IsSuccess)
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
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
        if (!result.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        }
        return Ok(result);
    }
    
    /// <summary>
    /// Personel Detayları Güncelleme Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Edit(WriteUpdatePersonalDto dto , string returnUrl)
    {
        var result = await _writePersonalService.UpdateAsync(dto);
        if (!result.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        }
        else
        {
            _toastNotification.AddSuccessToastMessage("Personel Başarılı Bir Şekilde Güncellendi", new ToastrOptions { Title = "Başarılı" }); 
        }

        return Redirect(returnUrl);
    }
    /// <summary>
    /// Personel Sil Post Metodu
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ArchivePersonal(Guid id)
    {
        var result = await _writePersonalService.DeleteAsync(id);
        if (!result.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        }
        else
        {
            _toastNotification.AddSuccessToastMessage("Personel Başarılı Bir Şekilde Silindi", new ToastrOptions { Title = "Başarılı" }); 
        }

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
        if (!result.IsSuccess)
        {
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "Hata" });
        }
        return Ok(result);
    }
    #endregion
  
   
   
}