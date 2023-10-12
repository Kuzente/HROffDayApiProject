using Core.Entities;
using Data.Abstract.PersonalRepositories;
using Data.Context;

namespace Data.Concrete.PersonalRepositories;

public class WritePersonalRepository : WriteRepository<Personal>, IWritePersonalRepository
{
	public WritePersonalRepository(DataContext context) : base(context)
	{
	}
}