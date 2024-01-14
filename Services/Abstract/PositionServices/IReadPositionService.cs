using Core.DTOs.BranchDTOs;
using Core.DTOs;
using Core.DTOs.PositionDTOs;
using Core.Interfaces;
using Core.Querys;

namespace Services.Abstract.PositionServices;

public interface IReadPositionService
{
	Task<bool> GetAnyByNameAsync(string name);
	Task<IResultWithDataDto<List<PositionDto>>> GetExcelPositionListService(PositionQuery query); // Aktif Ünvanlar Excel Servisi
	Task<List<PositionNameDto>> GetAllJustNames();
    Task<ResultWithPagingDataDto<List<PositionDto>>> GetPositionListService(int pageNumber, string search, bool  passive); // Ünvan Listesi Servisi
    Task<ResultWithPagingDataDto<List<PositionDto>>> GetDeletedPositionListService(int pageNumber,string search); // Silinen Ünvanlar Listesi Servisi
    Task<IResultWithDataDto<PositionDto>> GetUpdatePositionService(Guid id); // Ünvan Güncelleme Get Servisi
} 