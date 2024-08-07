using Core.DTOs;
using Core.DTOs.UserLogDTOs.ReadDtos;
using Core.Interfaces;
using Core.Querys;

namespace Services.Abstract.UserLogServices;

public interface IReadUserLogService
{
    Task<IResultWithDataDto<List<HeaderLastFiveLogDto>>> GetLastFiveLogService(); // Header son 5 log getirme servisi
	Task<ResultWithPagingDataDto<List<UsersLogListDto>>> GetUsersLogsListService(UserLogQuery query);
}