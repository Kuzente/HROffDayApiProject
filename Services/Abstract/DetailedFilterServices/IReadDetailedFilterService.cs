using Core.DTOs;
using Core.DTOs.DetailFilterDtos.ReadDtos;
using Core.Interfaces;

namespace Services.Abstract.DetailedFilterServices;

public interface IReadDetailedFilterService
{
    Task<IQueryable> GetDetailedFilterOdataService(string entityName);
}