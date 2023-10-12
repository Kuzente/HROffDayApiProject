using AutoMapper;
using Core.DTOs.OffDayDTOs;
using Data.Abstract;
using Services.Abstract.OffDayServices;

namespace Services.Concrete.OffDayServices;

public class ReadOffDayService : IReadOffDayService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public ReadOffDayService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<List<ReadOffDayDto>> GetAllAsync()
	{
		var entities = await Task.Run(() => _unitOfWork.ReadOffDayRepository.GetAll());
		return _mapper.Map<List<ReadOffDayDto>>(entities.ToList());
	}

	public Task<bool> GetAnyAsync()
	{
		throw new NotImplementedException();
	}

	public Task<ReadOffDayDto> GetSingleAsync()
	{
		throw new NotImplementedException();
	}
}