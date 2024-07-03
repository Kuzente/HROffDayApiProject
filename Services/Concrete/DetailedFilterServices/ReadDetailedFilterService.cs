using System.Linq.Expressions;
using System;
using System.Reflection;
using Core.DTOs;
using Core.DTOs.DetailFilterDtos.ReadDtos;
using Core.Entities;
using Core.Interfaces;
using Data.Abstract;
using Services.Abstract.DetailedFilterServices;
using Services.HelperServices;

namespace Services.Concrete.DetailedFilterServices;

public class ReadDetailedFilterService : IReadDetailedFilterService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReadDetailedFilterService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    
    public async Task<IQueryable> GetDetailedFilterOdataService(string entityName)
    {
        IQueryable query = null;

		query = entityName switch
		{
			"Personal" => _unitOfWork.ReadPersonalRepository.GetAll(),// Product ile ilgili sorgu
			"Branch" => _unitOfWork.ReadBranchRepository.GetAll(),// Order ile ilgili sorgu
            "Position" => _unitOfWork.ReadPositionRepository.GetAll(),
            "OffDay" => _unitOfWork.ReadOffDayRepository.GetAll(),
			_ => throw new ArgumentException("Unknown entity name"),// Varsayılan sorgu veya hata yönetimi
		};
		return query;
    }
   
    
}
