using Core.Entities;
using Data.Abstract;
using Data.Abstract.PositionRepositories;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Concrete.PositionRepositories
{
	public class WritePositionRepository : WriteRepository<Position>, IWritePositionRepository
	{
		public WritePositionRepository(DataContext context) : base(context)
		{
		}
	}
}
