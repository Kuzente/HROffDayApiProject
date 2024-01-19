using Core.DTOs;
using Core.DTOs.OffDayDTOs;
using Core.DTOs.OffDayDTOs.ReadDtos;
using Core.Interfaces;
using Core.Querys;

namespace Services.Abstract.OffDayServices;

public interface IReadOffDayService
{
    Task<ResultWithPagingDataDto<List<ReadWaitingOffDayListDto>>> GetFirstWaitingOffDaysListService(OffdayQuery query); //İlk onaylanacak bekleyen izinler liste servisi
    Task<IResultWithDataDto<ReadWaitingOffDayEditDto>> GetFirstWaitingOffDayByIdService(Guid id);// ilk onaylacanak bekleyen izinler düzenleme get servisi


}