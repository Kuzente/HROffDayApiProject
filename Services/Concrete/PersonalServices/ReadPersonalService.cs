using AutoMapper;
using Core.DTOs.PersonalDTOs;
using Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract.PersonalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete.PersonalServices
{
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
			ReadPersonalDto result = new ReadPersonalDto();
			var entities = await Task.Run(() => _unitOfWork.ReadPersonalRepository.GetAll());
			return _mapper.Map<List<ReadPersonalDto>>(entities.ToList());
		}

		public async Task<List<ReadPersonalDto>> GetAllWithBranchAndPositionAsync()
		{
			ReadPersonalDto result = new ReadPersonalDto();
			var entities = await Task.Run(() => _unitOfWork.ReadPersonalRepository.GetAll(include:p=> p.Include(a=>a.Branch).Include(b=>b.Position)));
			return _mapper.Map<List<ReadPersonalDto>>(entities.ToList());
		}

		public Task<bool> GetAnyAsync()
		{
			throw new NotImplementedException();
		}

		public Task<ReadPersonalDto> GetSingleAsync()
		{
			throw new NotImplementedException();
		}
	}
}
