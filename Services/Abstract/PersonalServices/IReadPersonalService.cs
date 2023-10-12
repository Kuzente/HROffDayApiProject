using Core.DTOs.PersonalDTOs;

namespace Services.Abstract.PersonalServices;

public interface IReadPersonalService : IReadService<ReadPersonalDto>
{
	Task<List<ReadPersonalDto>> GetAllWithBranchAndPositionAsync();
}