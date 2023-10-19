using System.Linq.Expressions;
using AutoMapper;
using Core.DTOs;
using Core.DTOs.BranchDTOs;
using Core.Interfaces;
using Data.Abstract;
using Services.Abstract.BranchServices;

namespace Services.Concrete.BranchServices;

public class ReadBranchService : IReadBranchService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public ReadBranchService(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}
	public async Task<List<ReadBranchDto>> GetAllAsync()
	{			
		var entities = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll());
		return _mapper.Map<List<ReadBranchDto>>(entities.ToList());
	}

	public Task<ReadBranchDto> GetSingleAsync()
	{
		throw new NotImplementedException(); 
	}

	public Task<bool> GetAnyAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<IResultWithDataDto<List<ReadBranchDto>>> GetAllOrderByAsync()
	{
		IResultWithDataDto<List<ReadBranchDto>> res = new ResultWithDataDto<List<ReadBranchDto>>();
		try
		{
			var resultData = await Task.Run(() => _unitOfWork.ReadBranchRepository.GetAll(orderBy: p=> p.OrderBy(a=>a.Name)));
			var mapData = _mapper.Map<List<ReadBranchDto>>(resultData.ToList());
			res.SetData(mapData);
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
				return res.SetStatus(false).SetErr("Commit Fail").SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}
		return res;
		
	}

	public async Task<ResultWithPagingDataDto<List<ReadBranchDto>>> GetAllPagingOrderByAsync(int pageNumber)
	{
		ResultWithPagingDataDto<List<ReadBranchDto>> res = new ResultWithPagingDataDto<List<ReadBranchDto>>(pageNumber:pageNumber);
		try
		{
			var allData = await Task.Run(() =>
				_unitOfWork.ReadBranchRepository.GetAll(
					orderBy: p => p.OrderBy(a => a.Name))
					
				);
			var resultData = allData.Skip((res.PageNumber - 1) * res.PageSize)
				.Take(res.PageSize).ToList();
			var mapData = _mapper.Map<List<ReadBranchDto>>(resultData);
			res.SetData(mapData);
			res.TotalRecords = allData.Count();
			res.TotalPages = Convert.ToInt32(Math.Ceiling((double)res.TotalRecords / (double)res.PageSize));
			var resultCommit = _unitOfWork.Commit();
			if (!resultCommit)
			{
				res.SetStatus(false).SetErr("Commit Fail")
					.SetMessage("Data kayıt edilemedi! Lütfen yaptığınız işlem bilgilerini kontrol ediniz...");
				return res;
			}
			
		}
		catch (Exception ex)
		{
			res.SetStatus(false).SetErr(ex.Message).SetMessage("İşleminiz sırasında bir hata meydana geldi! Lütfen daha sonra tekrar deneyin...");
		}

		return res;
	}
}