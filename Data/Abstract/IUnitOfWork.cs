using Data.Abstract.BranchRepositories;
using Data.Abstract.BranchUserRepositories;
using Data.Abstract.DailyFoodLogRepositories;
using Data.Abstract.DailyYearLogRepositories;
using Data.Abstract.MissingDayRepositories;
using Data.Abstract.OffDayRepositories;
using Data.Abstract.PersonalCumulativeRepositories;
using Data.Abstract.PersonalRepositories;
using Data.Abstract.PositionRepositories;
using Data.Abstract.TransferPersonalRepositories;
using Data.Abstract.UserLogRepositories;
using Data.Abstract.UserRepositories;

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
	IReadUserRepository ReadUserRepository { get;  }
	IWriteUserRepository WriteUserRepository { get;  }
	IReadBranchUserRepository ReadBranchUserRepository { get;  }
	IWriteBranchUserRepository WriteBranchUserRepository { get;  }
	IReadTransferPersonalRepository ReadTransferPersonalRepository { get;  }
	IWriteTransferPersonalRepository WriteTransferPersonalRepository { get;  }
	IReadMissingDayRepository ReadMissingDayRepository { get;  }
	IWriteMissingDayRepository WriteMissingDayRepository { get;  }
	IReadPersonalCumulativeRepository ReadPersonalCumulativeRepository { get;  }
	IWritePersonalCumulativeRepository WritePersonalCumulativeRepository { get;  }
	IWriteUserLogRepository WriteUserLogRepository { get;  }
	bool Commit(bool state = true);
}