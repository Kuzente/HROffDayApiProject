using Core.DTOs.MultipleUploadDtos;
using Core.DTOs.PersonalDTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Services.Abstract.ExcelServices;

public interface IReadExcelServices
{
	
	Task<IResultWithDataDto<List<AddRangePersonalDto>>> ImportPersonalUploadDataFromExcel(IFormFile file); //Toplu personel ekleme excel güncelleme metodu
    Task<IResultWithDataDto<List<SalaryUpdateDto>>> ImportSalaryUploadDataFromExcel(IFormFile file); //Toplu maaş güncelleme excel güncelleme metodu
    Task<IResultWithDataDto<List<IbanUpdateDto>>> ImportIbanUploadDataFromExcel(IFormFile file); //Toplu iban güncelleme excel güncelleme metodu
    Task<IResultWithDataDto<List<BankAccountUpdateDto>>> ImportBankAccountUploadDataFromExcel(IFormFile file); //Toplu banka hesabı güncelleme excel güncelleme metodu
}