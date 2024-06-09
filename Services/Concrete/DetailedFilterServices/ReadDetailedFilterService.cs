using System.Reflection;
using Core.DTOs;
using Core.DTOs.DetailFilterDtos.ReadDtos;
using Core.Entities;
using Core.Interfaces;
using Data.Abstract;
using Services.Abstract.DetailedFilterServices;

namespace Services.Concrete.DetailedFilterServices;

public class ReadDetailedFilterService : IReadDetailedFilterService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReadDetailedFilterService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResultWithDataDto<List<Dictionary<string, object>>>> GetFilteredResultService(ReadDetailFilterDto dto)
    {
        IResultWithDataDto<List<Dictionary<string, object>>> result = new ResultWithDataDto<List<Dictionary<string, object>>>();
        try
        {
            
        }
        catch (Exception ex)
        {
            result.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
        }

        return result;
    }
    public async Task<IQueryable> GetDetailedFilterOdataService(string entityName)
    {
        IQueryable query = null;

        switch (entityName)
        {
            case "Personal":
                // Product ile ilgili sorgu
                query = _unitOfWork.ReadPersonalRepository.GetAll();
                break;
            case "Branch":
                // Order ile ilgili sorgu
                query = _unitOfWork.ReadBranchRepository.GetAll();
                break;
            default:
                // Varsayılan sorgu veya hata yönetimi
                throw new ArgumentException("Unknown entity name");
        }
        return query;
    }
   
    
}
