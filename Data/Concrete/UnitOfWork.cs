﻿using Data.Abstract;
using Data.Abstract.BranchRepositories;
using Data.Abstract.DailyCounterRepositories;
using Data.Abstract.OffDayRepositories;
using Data.Abstract.PersonalRepositories;
using Data.Abstract.PositionRepositories;
using Data.Concrete.BranchRepositories;
using Data.Concrete.DailyCounterRepositories;
using Data.Concrete.OffDayRepositories;
using Data.Concrete.PersonalRepositories;
using Data.Concrete.PositionRepositories;
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
	public IWriteDailyCounterRepository WriteDailyCounterRepository { get; private set; }
	public IReadDailyCounterRepository ReadDailyCounterRepository { get; private set; }

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
		ReadDailyCounterRepository = new ReadDailyCounterRepository(_context);
		WriteDailyCounterRepository = new WriteDailyCounterRepository(_context);
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