using Core.Entities;
using Data.Abstract.BranchRepositories;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Concrete.BranchRepositories
{
	public class ReadBranchRepository : ReadRepository<Branch>, IReadBranchRepository
	{
		public ReadBranchRepository(DataContext context) : base(context)
		{
		}
	}
}
