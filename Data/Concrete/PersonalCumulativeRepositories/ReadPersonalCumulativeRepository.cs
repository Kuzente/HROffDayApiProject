using Core.Entities;
using Data.Abstract.PersonalCumulativeRepositories;
using Data.Context;

namespace Data.Concrete.PersonalCumulativeRepositories;

public class ReadPersonalCumulativeRepository : ReadRepository<PersonalCumulative>, IReadPersonalCumulativeRepository
{
    public ReadPersonalCumulativeRepository(DataContext context) : base(context)
    {
    }
}