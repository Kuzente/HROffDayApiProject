using AutoMapper;
using Core.DTOs.PositionDTOs;
using Data.Abstract;
using Services.Abstract.PositionServices;

namespace Services.Concrete.PositionServices
{
	public class ReadPositionService : IReadPositionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ReadPositionService(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public async Task<List<ReadPositionDto>> GetAllAsync()
		{
			var entities = await Task.Run(() => _unitOfWork.ReadPositionRepository.GetAll());
			return _mapper.Map<List<ReadPositionDto>>(entities.ToList());
		}

		public Task<bool> GetAnyAsync()
		{
			throw new NotImplementedException();
		}

		public Task<ReadPositionDto> GetSingleAsync()
		{
			throw new NotImplementedException();
		}
	}
}
