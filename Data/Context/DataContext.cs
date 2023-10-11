using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
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
				if (EntityState.Added == data.State)
				{
					data.Entity.CreatedAt = data.Entity.CreatedAt.Year > 1000 ? data.Entity.CreatedAt : DateTime.Now;
					data.Entity.ModifiedAt = data.Entity.ModifiedAt.Year > 1000 ? data.Entity.ModifiedAt : DateTime.Now;
					data.Entity.Status = Core.Enums.EntityStatusEnum.Online;
				}
				else if (EntityState.Modified == data.State)
				{
					data.Entity.ModifiedAt = data.Entity.ModifiedAt.Year > 1000 ? data.Entity.ModifiedAt : DateTime.Now;
				}
					
				else if (EntityState.Deleted == data.State)
				{
					data.Entity.DeletedAt = data.Entity.DeletedAt.Year > 1000 ? data.Entity.DeletedAt : DateTime.Now;
					data.Entity.Status = Core.Enums.EntityStatusEnum.Deleted;
				}
					
			}
			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
