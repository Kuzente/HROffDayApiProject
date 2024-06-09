using Core.DTOs.DetailFilterDtos.ReadDtos;
using Core.Interfaces;

namespace Services.Abstract.DetailedFilterServices;

public interface IReadDetailedFilterService
{
    Task<IResultWithDataDto<List<Dictionary<string, object>>>> GetFilteredResultService(ReadDetailFilterDto dto);
    Task<IQueryable> GetDetailedFilterOdataService(string entityName);
}