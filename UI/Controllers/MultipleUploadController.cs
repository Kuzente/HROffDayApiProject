using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract.BranchServices;
using Services.Abstract.ExcelServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;
using Services.ExcelDownloadServices.MultipleUploadServices;
using UI.Helpers;

namespace UI.Controllers;

[Authorize(Roles = $"{nameof(UserRoleEnum.HumanResources)},{nameof(UserRoleEnum.SuperAdmin)}")]
public class MultipleUploadController : BaseController
{
    private readonly IWritePersonalService _writePersonalService;
    private readonly IReadBranchService _readBranchService;
    private readonly IReadPositionService _readPositionService;
    private readonly ExcelUploadScheme _excelUploadScheme;
    private readonly IReadExcelServices _readExcelServices;
    private readonly IReadPersonalService _readPersonalService;
    private readonly SalaryExcelUploadScheme _salaryExcelUploadScheme;
    private readonly IbanExcelUploadScheme _ibanExcelUploadScheme;
    private readonly BankAccountExcelUploadScheme _bankAccountExcelUploadScheme;

	public MultipleUploadController(
		IWritePersonalService writePersonalService, IReadPositionService readPositionService,
		IReadBranchService readBranchService, ExcelUploadScheme excelUploadScheme, IReadExcelServices readExcelServices, IReadPersonalService readPersonalService, SalaryExcelUploadScheme salaryExcelUploadScheme, IbanExcelUploadScheme ibanExcelUploadScheme, BankAccountExcelUploadScheme bankAccountExcelUploadScheme)
	{
		_writePersonalService = writePersonalService;
		_readPositionService = readPositionService;
		_readBranchService = readBranchService;
		_excelUploadScheme = excelUploadScheme;
		_readExcelServices = readExcelServices;
		_readPersonalService = readPersonalService;
		_salaryExcelUploadScheme = salaryExcelUploadScheme;
		_ibanExcelUploadScheme = ibanExcelUploadScheme;
		_bankAccountExcelUploadScheme = bankAccountExcelUploadScheme;
	}

	#region Get Methods

	/// <summary>
	/// Toplu Personel Ekleme Sayfası
	/// </summary>
	/// <returns></returns>
	public IActionResult PersonalUpload()
    {
        return View();
    }
	/// <summary>
	/// Toplu Personel Ekleme Taslak Excel İndirme Metodu
	/// </summary>
	/// <returns></returns>
	public async Task<IActionResult> GetExcelUploadSheme()
	{
		var branches = await _readBranchService.GetAllJustNames();
		var positions = await _readPositionService.GetAllJustNames();
		string personalStaticSelects = PersonalStaticSelectsConverter.JsonToString();

		byte[] excelData =
			_excelUploadScheme.ExportToExcel(positions, branches, personalStaticSelects); // Entity listesini Excel verisi olarak alın.

		var response = HttpContext.Response;
		response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
		response.Headers.Add("Content-Disposition", "attachment; filename=TopluVeriTaslak.xlsx");
		await response.Body.WriteAsync(excelData, 0, excelData.Length);
		return new EmptyResult();
	}
	/// <summary>
	/// Toplu Personel Maaş Güncelleme Taslak Excel İndirme Metodu
	/// </summary>
	/// <returns></returns>
	public async Task<IActionResult> GetExcelSalaryUploadSheme()
	{
		var personals = await _readPersonalService.GetPersonalsSalary();
		if (!personals.IsSuccess)
			return Ok(personals);

		byte[] excelData =
			_salaryExcelUploadScheme.ExportToExcel(personals.Data);

		var response = HttpContext.Response;
		response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
		response.Headers.Add("Content-Disposition", "attachment; filename=TopluMaasTaslak.xlsx");
		await response.Body.WriteAsync(excelData, 0, excelData.Length);
		return new EmptyResult();
	}
	/// <summary>
	/// Toplu Personel IBAN Güncelleme Taslak Excel İndirme Metodu
	/// </summary>
	/// <returns></returns>
	public async Task<IActionResult> GetExcelIBANUploadSheme()
	{
		var personals = await _readPersonalService.GetPersonalsIbans();
		if (!personals.IsSuccess)
			return Ok(personals);

		byte[] excelData =
			_ibanExcelUploadScheme.ExportToExcel(personals.Data);

		var response = HttpContext.Response;
		response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
		response.Headers.Add("Content-Disposition", "attachment; filename=TopluIBANTaslak.xlsx");
		await response.Body.WriteAsync(excelData, 0, excelData.Length);
		return new EmptyResult();
	}
	/// <summary>
	/// Toplu Personel Banka Hesabı Güncelleme Taslak Excel İndirme Metodu
	/// </summary>
	/// <returns></returns>
	public async Task<IActionResult> GetExcelBankAccountUploadSheme()
	{
		var personals = await _readPersonalService.GetPersonalsBankAccounts();
		if (!personals.IsSuccess)
			return Ok(personals);

		byte[] excelData =
			_bankAccountExcelUploadScheme.ExportToExcel(personals.Data);

		var response = HttpContext.Response;
		response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
		response.Headers.Add("Content-Disposition", "attachment; filename=TopluBankaHesabiTaslak.xlsx");
		await response.Body.WriteAsync(excelData, 0, excelData.Length);
		return new EmptyResult();
	}
	#endregion

	#region Post Methods

	/// <summary>
	/// Toplu Personel Ekleme Post Metodu 
	/// </summary>
	/// <returns></returns>
	[HttpPost]
    public async Task<IActionResult> PersonalUpload(IFormFile file)
    {
        var resultExcel = await _readExcelServices.ImportPersonalUploadDataFromExcel(file);
        if (!resultExcel.IsSuccess) return Ok(resultExcel);
        if (!GetClientUserId().HasValue) return Redirect("/404"); // Veya uygun bir hata sayfası
        var result = await _writePersonalService.AddRangeAsync(resultExcel.Data,GetClientUserId()!.Value,GetClientIpAddress());
        

        return Ok(result);
    }
	/// <summary>
	/// Toplu Maaş Güncelleme Post Metodu 
	/// </summary>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> SalaryUpload(IFormFile file)
	{
		var resultExcel = await _readExcelServices.ImportSalaryUploadDataFromExcel(file);
		if (!resultExcel.IsSuccess)
			return Ok(resultExcel);
		if (!GetClientUserId().HasValue)
			return Redirect("/404"); // Veya uygun bir hata sayfası
		var result = await _writePersonalService.UpdateMultiplePersonalSalaryAsyncService(resultExcel.Data, GetClientUserId()!.Value, GetClientIpAddress());
		return Ok(result);
	}
	/// <summary>
	/// Toplu IBAN Güncelleme Post Metodu 
	/// </summary>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> IbanUpload(IFormFile file)
	{
		var resultExcel = await _readExcelServices.ImportIbanUploadDataFromExcel(file);
		if (!resultExcel.IsSuccess)
			return Ok(resultExcel);
		if (!GetClientUserId().HasValue)
			return Redirect("/404"); // Veya uygun bir hata sayfası
		var result = await _writePersonalService.UpdateMultiplePersonalIbanAsyncService(resultExcel.Data, GetClientUserId()!.Value, GetClientIpAddress());
		return Ok(result);
	}
	/// <summary>
	/// Toplu Banka Hesabı Güncelleme Post Metodu 
	/// </summary>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> BankAccountUpload(IFormFile file)
	{
		var resultExcel = await _readExcelServices.ImportBankAccountUploadDataFromExcel(file);
		if (!resultExcel.IsSuccess)
			return Ok(resultExcel);
		if (!GetClientUserId().HasValue)
			return Redirect("/404"); // Veya uygun bir hata sayfası
		var result = await _writePersonalService.UpdateMultiplePersonalBankAccountAsyncService(resultExcel.Data, GetClientUserId()!.Value, GetClientIpAddress());
		return Ok(result);
	}
	#endregion
}