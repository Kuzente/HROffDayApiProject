using Core.DTOs;
using Core.DTOs.TransferPersonalDtos.ReadDtos;
using Core.Interfaces;
using Core.Querys;

namespace Services.Abstract.TransferPersonalService;

public interface IReadTransferPersonalService
{
    Task<ResultWithPagingDataDto<List<ReadTransferPersonalDto>>> GetTransferPersonalListByIdService(TransferPersonalQuery query); // Personel Detayları Nakil Listesi Servisi
    Task<ResultWithPagingDataDto<List<ReadTransferPersonalDto>>> GetTransferPersonalListService(TransferPersonalQuery query); // Personel Detayları Nakil Listesi Servisi

    Task<IResultWithDataDto<List<ReadTransferPersonalDto>>> ExcelGetTransferPersonalListByIdService(TransferPersonalQuery query);// Personel Detayları Nakil Listesi Excel Servisi
    Task<IResultWithDataDto<List<ReadTransferPersonalDto>>> ExcelGetTransferPersonalListService(TransferPersonalQuery query);// Personel Nakil Listesi Excel Servisi
}