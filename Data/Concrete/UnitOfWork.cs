using Data.Abstract;
using Data.Abstract.BranchRepositories;
using Data.Abstract.BranchUserRepositories;
using Data.Abstract.DailyFoodLogRepositories;
using Data.Abstract.DailyYearLogRepositories;
using Data.Abstract.OffDayRepositories;
using Data.Abstract.PersonalRepositories;
using Data.Abstract.PositionRepositories;
using Data.Abstract.TransferPersonalRepositories;
using Data.Abstract.UserRepositories;
using Data.Concrete.BranchRepositories;
using Data.Concrete.BranchUserRepositories;
using Data.Concrete.DailyFoodLogRepositories;
using Data.Concrete.DailyYearLogRepositories;
using Data.Concrete.OffDayRepositories;
using Data.Concrete.PersonalRepositories;
using Data.Concrete.PositionRepositories;
using Data.Concrete.TransferPersonalRepositories;
using Data.Concrete.UserRepositories;
using Data.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Concrete;

public class UnitOfWork : IUnitOfWork
{
	private readonly DataContext _context;
	private readonly IDbContextTransaction _transaction;

	public IWritePersonalRepository WritePersonalRepository { get; private set; }
	public IReadPersonalRepository ReadPersonalRepository { get; private set; }

	public IWriteBranchRepository WriteBranchRepository { get; private set; }

	public IReadBranchRepository ReadBranchRepository { get; private set; }

	public IWritePositionRepository WritePositionRepository { get; private set; }

	public IReadPositionRepository ReadPositionRepository { get; private set; }
	public IWriteOffDayRepository WriteOffDayRepository { get; private set; }
	public IReadOffDayRepository ReadOffDayRepository { get; private set; }
	public IWriteDailyYearLogRepository WriteDailyYearLogRepository { get; private set; }
	public IReadDailyYearLogRepository ReadDailyYearLogRepository { get; private set; }
	public IWriteDailyFoodLogRepository WriteDailyFoodLogRepository { get; private set; }
	public IReadDailyFoodLogRepository ReadDailyFoodLogRepository { get; private set; }
	public IWriteUserRepository WriteUserRepository { get; private set; }
	public IReadUserRepository ReadUserRepository { get; private set; }
	public IWriteBranchUserRepository WriteBranchUserRepository { get; private set; }
	public IReadBranchUserRepository ReadBranchUserRepository { get; private set; }
	public IWriteTransferPersonalRepository WriteTransferPersonalRepository { get; private set; }
	public IReadTransferPersonalRepository ReadTransferPersonalRepository { get; private set; }
	

	public UnitOfWork(DataContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		WritePersonalRepository = new WritePersonalRepository(_context);
		ReadPersonalRepository = new ReadPersonalRepository(_context);
		WriteBranchRepository = new WriteBranchRepository(_context);
		ReadBranchRepository = new ReadBranchRepository(_context);
		WritePositionRepository = new WritePositionRepository(_context);
		ReadPositionRepository = new ReadPositionRepository(_context);
		ReadOffDayRepository = new ReadOffDayRepository(_context);
		WriteOffDayRepository = new WriteOffDayRepository(_context);
		ReadDailyYearLogRepository = new ReadDailyYearLogRepository(_context);
		WriteDailyYearLogRepository = new WriteDailyYearLogRepository(_context);
		ReadDailyFoodLogRepository = new ReadDailyFoodLogRepository(_context);
		WriteDailyFoodLogRepository = new WriteDailyFoodLogRepository(_context);
		ReadUserRepository = new ReadUserRepository(_context);
		WriteUserRepository = new WriteUserRepository(_context);
		ReadBranchUserRepository = new ReadBranchUserRepository(_context);
		WriteBranchUserRepository = new WriteBranchUserRepository(_context);
		ReadTransferPersonalRepository = new ReadTransferPersonalRepository(_context);
		WriteTransferPersonalRepository = new WriteTransferPersonalRepository(_context);
		_transaction = _context.Database.BeginTransaction();
	}

	public bool Commit(bool state = true)
	{
		_context.SaveChanges();
		if (state)
		{
			_transaction.Commit();
			Dispose();
			return true;
		}

		_transaction.Rollback();
		Dispose();
		return false;
	}

	public void Dispose() => _context.Dispose();

}