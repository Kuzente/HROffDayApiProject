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
	public class WriteOffDayRepository : WriteRepository<OffDay>, IWriteOffDayRepository
	{
		public WriteOffDayRepository(DataContext context) : base(context)
		{
		}
	}
}
