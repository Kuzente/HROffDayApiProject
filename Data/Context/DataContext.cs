﻿using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options) { }

	public DbSet<Personal> Personals => Set<Personal>();
	public DbSet<Branch> Branches => Set<Branch>();
	public DbSet<Position> Positions => Set<Position>();
	public DbSet<OffDay> OffDays => Set<OffDay>();
	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var datas = ChangeTracker.Entries<BaseEntity>();
		foreach (var data in datas)
		{
			switch (data.State)
			{
				case EntityState.Added:
					data.Entity.CreatedAt = data.Entity.CreatedAt.Year > 1000 ? data.Entity.CreatedAt : DateTime.Now;
					data.Entity.ModifiedAt = data.Entity.ModifiedAt.Year > 1000 ? data.Entity.ModifiedAt : DateTime.Now;
					data.Entity.Status = Core.Enums.EntityStatusEnum.Online;
					break;
				case EntityState.Modified:
					data.Entity.ModifiedAt = data.Entity.ModifiedAt.Year > 1000 ? data.Entity.ModifiedAt : DateTime.Now;
					break;
				case EntityState.Deleted:
					data.Entity.DeletedAt = data.Entity.DeletedAt.Year > 1000 ? data.Entity.DeletedAt : DateTime.Now;
					data.Entity.Status = Core.Enums.EntityStatusEnum.Deleted;
					break;
			}
		}
		return base.SaveChangesAsync(cancellationToken);
	}

}