using Core.Entities;
using Data.Abstract.PersonalCumulativeRepositories;
using Data.Context;

namespace Data.Concrete.PersonalCumulativeRepositories;

public class WritePersonalCumulativeRepository : WriteRepository<PersonalCumulative>, IWritePersonalCumulativeRepository
{
    public WritePersonalCumulativeRepository(DataContext context) : base(context)
    {
    }
}