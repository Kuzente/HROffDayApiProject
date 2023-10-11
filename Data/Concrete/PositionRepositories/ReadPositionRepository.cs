using Core.Entities;
using Data.Abstract.PositionRepositories;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Concrete.PositionRepositories
{
	public class ReadPositionRepository : ReadRepository<Position>, IReadPositionRepository
	{
		public ReadPositionRepository(DataContext context) : base(context)
		{
		}
	}
}
