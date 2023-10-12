using Core.Entities;
using Data.Abstract.PersonalRepositories;
using Data.Context;

namespace Data.Concrete.PersonalRepositories;

public class ReadPersonalRepository : ReadRepository<Personal>, IReadPersonalRepository
{
	public ReadPersonalRepository(DataContext context) : base(context)
	{
	}
}