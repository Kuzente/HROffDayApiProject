using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context;

public class DataContext : DbContext
{
	private readonly IConfiguration _configuration;
	public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
	{
		_configuration = configuration;
	}
	public DbSet<Personal> Personals => Set<Personal>();
	public DbSet<PersonalDetails> PersonalDetails => Set<PersonalDetails>();
	public DbSet<Branch> Branches => Set<Branch>();
	public DbSet<Position> Positions => Set<Position>();
	public DbSet<OffDay> OffDays => Set<OffDay>();
	public DbSet<DailyYearLog> DailyYearLogs => Set<DailyYearLog>();
	public DbSet<DailyFoodLog> DailyFoodLogs => Set<DailyFoodLog>();
	public DbSet<User> Users => Set<User>();
	public DbSet<BranchUser> BranchUsers => Set<BranchUser>();
	public DbSet<TransferPersonal> TransferPersonals => Set<TransferPersonal>();
	public DbSet<MissingDay> MissingDays => Set<MissingDay>();
	public DbSet<PersonalCumulative> PersonalCumulatives => Set<PersonalCumulative>();
	public DbSet<UserLog> UserLogs => Set<UserLog>();
	public override int SaveChanges()
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
		return base.SaveChanges();
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<MissingDay>()
			.HasOne(m => m.Branch)
			.WithMany(b => b.MissingDays)
			.HasForeignKey(m => m.Branch_Id)
			.OnDelete(DeleteBehavior.Restrict);
		modelBuilder.Seed(_configuration.GetSection("SuperadminPass").Value);
	}



}