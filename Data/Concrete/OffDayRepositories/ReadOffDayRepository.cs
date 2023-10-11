using Core.Entities;
using Data.Abstract.OffDayRepositories;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Concrete.OffDayRepositories
{
	public class ReadOffDayRepository : ReadRepository<OffDay>, IReadOffDayRepository
	{
		public ReadOffDayRepository(DataContext context) : base(context)
		{
		}
	}
}
