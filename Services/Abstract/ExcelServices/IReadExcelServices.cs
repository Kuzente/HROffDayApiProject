using Core.DTOs.PersonalDTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Services.Abstract.ExcelServices;

public interface IReadExcelServices
{
    Task<IResultWithDataDto<List<AddRangePersonalDto>>> ImportDataFromExcel(IFormFile file);
}