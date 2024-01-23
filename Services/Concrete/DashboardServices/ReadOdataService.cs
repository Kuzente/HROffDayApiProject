using Core.Enums;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.DashboardServices;

namespace Services.Concrete.DashboardServices;

public class ReadOdataService : IReadOdataService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReadOdataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IQueryable> GetBranchesOdataService()
    {
        var query = _unitOfWork.ReadBranchRepository.GetAll(predicate: p =>
            p.Status == EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline);
	    
        return query;
    }

    public async Task<IQueryable> GetPositionOdataService()
    {
        var query = _unitOfWork.ReadPositionRepository.GetAll(predicate: p =>
            p.Status == EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline);
	    
        return query;
    }

    public async Task<IQueryable> GetPersonalOdataService()
    {
        var query = _unitOfWork.ReadPersonalRepository.GetAll(predicate: p =>
            p.Status == EntityStatusEnum.Online || p.Status == EntityStatusEnum.Offline);
        return query;
    }
}