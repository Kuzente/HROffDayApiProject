using Core.DTOs;
using Core.DTOs.OffDayDTOs;
using Core.DTOs.OffDayDTOs.ReadDtos;
using Core.Interfaces;
using Core.Querys;

namespace Services.Abstract.OffDayServices;

public interface IReadOffDayService
{
    Task<ResultWithPagingDataDto<List<ReadWaitingOffDayListDto>>> GetFirstWaitingOffDaysListService(OffdayQuery query); //İlk onaylanacak bekleyen izinler liste servisi
    Task<ResultWithPagingDataDto<List<ReadWaitingOffDayListDto>>> GetSecondWaitingOffDaysListService(OffdayQuery query); //İkinci onaylanacak bekleyen izinler liste servisi(Tümü)
    Task<ResultWithPagingDataDto<List<ReadRejectedOffDayListDto>>> GetRejectedOffDaysListService(OffdayQuery query); //Reddedilen izinler liste servisi
    Task<ResultWithPagingDataDto<List<ReadApprovedOffDayListDto>>> GetApprovedOffDaysListService(OffdayQuery query); //Onaylanan izinler liste servisi
    Task<ResultWithPagingDataDto<List<ReadDeletedOffDayListDto>>> GetDeletedOffDaysListService(OffdayQuery query); //Silinen izinler liste servisi
    Task<ResultWithPagingDataDto<List<ReadPersonalOffDayListDto>>> GetPersonalOffDaysListService(OffdayQuery query); //Personele göre izinler liste servisi
    Task<IResultWithDataDto<ReadWaitingOffDayEditDto>> GetFirstWaitingOffDayByIdService(Guid id);// ilk onaylacanak bekleyen izinler düzenleme get servisi
    Task<IResultWithDataDto<List<ReadApprovedOffDayListDto>>> GetExcelApprovedOffDayListService(OffdayQuery query);// Excel Alma Servisi
    Task<IResultWithDataDto<ReadApprovedOffDayFormExcelExportDto>> GetApprovedOffDayExcelFormService(Guid id);// Onaylanan İzin Form Get Servisi


}