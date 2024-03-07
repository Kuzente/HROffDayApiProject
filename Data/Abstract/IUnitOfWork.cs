using Data.Abstract.BranchRepositories;
using Data.Abstract.DailyFoodLogRepositories;
using Data.Abstract.DailyYearLogRepositories;
using Data.Abstract.OffDayRepositories;
using Data.Abstract.PersonalRepositories;
using Data.Abstract.PositionRepositories;

namespace Data.Abstract;

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
	IReadDailyYearLogRepository ReadDailyYearLogRepository { get;  }
	IWriteDailyYearLogRepository WriteDailyYearLogRepository { get;  }
	IReadDailyFoodLogRepository ReadDailyFoodLogRepository { get;  }
	IWriteDailyFoodLogRepository WriteDailyFoodLogRepository { get;  }
	bool Commit(bool state = true);
}