using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.DTOs.PersonalDTOs;
using Core.Enums;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.PersonalServices;

namespace Services.Concrete.PersonalServices;

public class ReadPersonalService : IReadPersonalService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public ReadPersonalService(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<List<ReadPersonalDto>> GetAllAsync()
	{
		var entities = await Task.Run(() => _unitOfWork.ReadPersonalRepository.GetAll());
		return _mapper.Map<List<ReadPersonalDto>>(entities.ToList());
	}

	public async Task<List<ReadPersonalDto>> GetAllWithBranchAndPositionAsync()
	{
		var entities = await Task.Run(() => _unitOfWork.ReadPersonalRepository.GetAll(
			include:p=> p
			.Include(a=>a.Branch)
			.Include(b=>b.Position),
			orderBy: o=> o
			.OrderBy(o=>o.NameSurname)
			)
		);
		return _mapper.Map<List<ReadPersonalDto>>(entities.ToList());
	}

	public async Task<ResultWithPagingDataDto<List<ReadPersonalDto>>> GetAllPagingWithBranchAndPositionOrderByAsync(int pageNumber, string search)
	{
		ResultWithPagingDataDto<List<ReadPersonalDto>> res = new ResultWithPagingDataDto<List<ReadPersonalDto>>(pageNumber,search);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadPersonalRepository.GetAll(
					include:p=> p
						.Include(a=>a.Branch)
						.Include(b=>b.Position),
					orderBy: p => p.OrderBy(a => a.NameSurname),
					predicate: a => (a.Status == EntityStatusEnum.Online || a.Status == EntityStatusEnum.Offline) && 
					                (string.IsNullOrEmpty(search) || a.NameSurname.Contains(search))
				));   
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<ReadPersonalDto>>(resultData);
			res.SetData(mapData);
			res.TotalRecords = allData.Count();
			res.TotalPages = Convert.ToInt32(Math.Ceiling((double)res.TotalRecords / (double)res.PageSize));
			
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}

	public Task<bool> GetAnyAsync()
	{
		throw new NotImplementedException(); //TODO
	}

	public Task<ReadPersonalDto> GetSingleAsync()
	{
		throw new NotImplementedException(); //TODO
	}

	public Task<bool> GetAnyAsync(Expression<Func<ReadPersonalDto, bool>>? predicate = null)
	{
		throw new NotImplementedException();
	}
}