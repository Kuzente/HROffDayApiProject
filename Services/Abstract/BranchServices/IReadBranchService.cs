using Core.DTOs;
using Core.DTOs.BaseDTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.BranchDTOs.ReadDtos;
using Core.Interfaces;
using Core.Querys;

namespace Services.Abstract.BranchServices;

public interface IReadBranchService
{
	Task<IResultWithDataDto<List<BranchDto>>> GetExcelBranchListService(BranchQuery query); // Aktif Şubeler Excel Servisi
	Task<ResultWithPagingDataDto<List<BranchDto>>> GetBranchListService(BranchQuery query); // Şube Listesi Servisi
	Task<ResultWithPagingDataDto<List<BranchDto>>> GetDeletedBranchListService(BranchQuery query); // Silinen Şubeler Listesi Servisi
	Task<IResultWithDataDto<BranchDto>> GetUpdateBranchService(Guid id); // Şube Güncelleme Get Servisi
    Task<List<BranchNameDto>> GetAllJustNames();

	Task<IResultWithDataDto<List<DepartmentCountDto>>> GetDepartmantCountsByBranchService(); // Aktif Şubeler Excel Servisi

}