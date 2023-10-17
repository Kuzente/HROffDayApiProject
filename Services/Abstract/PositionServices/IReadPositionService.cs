using Core.DTOs.PositionDTOs;

namespace Services.Abstract.PositionServices;

public interface IReadPositionService : IReadService<ReadPositionDto>
{
	Task<bool> GetAnyByNameAsync(string name);
}