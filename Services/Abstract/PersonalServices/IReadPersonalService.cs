using Core.DTOs;
using Core.DTOs.OffDayDTOs.ReadDtos;
using Core.DTOs.PassivePersonalDtos;
using Core.DTOs.PersonalDetailDto.ReadDtos;
using Core.DTOs.PersonalDTOs;
using Core.DTOs.PersonalDTOs.ReadDtos;
using Core.Interfaces;
using Core.Querys;

namespace Services.Abstract.PersonalServices;

public interface IReadPersonalService
{
	Task<IResultWithDataDto<List<PersonalDto>>> GetExcelPersonalListService(PersonalQuery query); // Aktif Personel Excel Servisi
	Task<IResultWithDataDto<List<PassivePersonalDto>>> GetExcelPassivePersonalListService(PersonalQuery query); // Pasif Personel Excel Servisi
	Task<IResultWithDataDto<ReadUpdatePersonalDto>> GetUpdatePersonalService(Guid id); // Personel Düzenleme Get Servisi
	Task<ResultWithPagingDataDto<List<PersonalDto>>> GetPersonalListService(PersonalQuery query); // Aktif Personel Listesi Servisi
	Task<ResultWithPagingDataDto<List<PassivePersonalDto>>> GetPassivePersonalListService(PersonalQuery query); // Pasif Personel Listesi Servisi
	Task<ResultWithPagingDataDto<List<PersonalDto>>> GetDeletedPersonalListService(PersonalQuery query); // Silinen Personel Listesi Servisi
	Task<IResultWithDataDto<List<ReadPersonalsByBranchIdDto>>> GetAllPersonalByBranchIdService(Guid branchId); // İzin Ekleme Şube ID ye göre personel getiren Servis
	Task<IResultWithDataDto<ReadPersonalDetailsHeaderDto>> GetPersonalDetailsHeaderByIdService(Guid id); // Personel Detayları Üst Menü Servisi
}