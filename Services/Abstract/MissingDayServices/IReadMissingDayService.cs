using Core.DTOs;
using Core.DTOs.MissingDayDtos.ReadDtos;
using Core.DTOs.TransferPersonalDtos.ReadDtos;
using Core.Interfaces;
using Core.Querys;

namespace Services.Abstract.MissingDayServices;

public interface IReadMissingDayService
{
    Task<ResultWithPagingDataDto<List<ReadMissingDayDto>>> GetMissingDayListByIdService(MissingDayQuery query); // Personel Detayları Eksik Gün Listesi Servisi
    Task<IResultWithDataDto<List<ReadMissingDayDto>>> ExcelGetPersonalMissingDayListByIdService(MissingDayQuery query);// Personel Detayları Eksik Gün Listesi Excel Servisi
}