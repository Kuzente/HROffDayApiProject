using Core.Entities;
using Data.Abstract.PersonalRepositories;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Concrete.PersonalRepositories
{
	public class ReadPersonalRepository : ReadRepository<Personal>, IReadPersonalRepository
	{
		public ReadPersonalRepository(DataContext context) : base(context)
		{
		}
	}
}
