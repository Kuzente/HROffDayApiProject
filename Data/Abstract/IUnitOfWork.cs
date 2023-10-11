using Data.Abstract.BranchRepositories;
using Data.Abstract.OffDayRepositories;
using Data.Abstract.PersonalRepositories;
using Data.Abstract.PositionRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Abstract
{
	public interface IUnitOfWork : IDisposable
	{
		IWritePersonalRepository WritePersonalRepository { get; }
		IReadPersonalRepository ReadPersonalRepository { get; }
		IWriteBranchRepository WriteBranchRepository { get; }
		IReadBranchRepository ReadBranchRepository { get; }
		IWritePositionRepository WritePositionRepository { get; }
		IReadPositionRepository ReadPositionRepository { get; }
		IWriteOffDayRepository WriteOffDayRepository { get;  }
		IReadOffDayRepository ReadOffDayRepository { get;  }
		bool Commit(bool state = true);
	}
}
