using Data.Abstract;
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
using Data.Concrete;
using Data.Concrete.BranchRepositories;
using Data.Concrete.BranchUserRepositories;
using Data.Concrete.DailyFoodLogRepositories;
using Data.Concrete.DailyYearLogRepositories;
using Data.Concrete.MissingDayRepositories;
using Data.Concrete.OffDayRepositories;
using Data.Concrete.PersonalCumulativeRepositories;
using Data.Concrete.PersonalRepositories;
using Data.Concrete.PositionRepositories;
using Data.Concrete.TransferPersonalRepositories;
using Data.Concrete.UserLogRepositories;
using Data.Concrete.UserRepositories;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class ServiceRegistration
{
	public static void AddDataLayerService(this IServiceCollection services , string? connectionstring)
	{
		services.AddDbContext<DataContext>(opt => opt.UseSqlServer(connectionstring));
		services.AddScoped<IUnitOfWork,UnitOfWork>();
		services.AddScoped(typeof(IReadRepository<>),typeof(ReadRepository<>));
		services.AddScoped(typeof(IWriteRepository<>),typeof(WriteRepository<>));
		services.AddScoped(typeof(IReadBranchRepository),typeof(ReadBranchRepository));
		services.AddScoped(typeof(IWriteBranchRepository),typeof(WriteBranchRepository));
		services.AddScoped(typeof(IReadOffDayRepository),typeof(ReadOffDayRepository));
		services.AddScoped(typeof(IWriteOffDayRepository),typeof(WriteOffDayRepository));
		services.AddScoped(typeof(IReadPersonalRepository),typeof(ReadPersonalRepository));
		services.AddScoped(typeof(IWritePersonalRepository),typeof(WritePersonalRepository));
		services.AddScoped(typeof(IReadPositionRepository),typeof(ReadPositionRepository));
		services.AddScoped(typeof(IWritePositionRepository),typeof(WritePositionRepository));
		services.AddScoped(typeof(IReadDailyYearLogRepository),typeof(ReadDailyYearLogRepository));
		services.AddScoped(typeof(IWriteDailyYearLogRepository),typeof(WriteDailyYearLogRepository));
		services.AddScoped(typeof(IReadDailyFoodLogRepository),typeof(ReadDailyFoodLogRepository));
		services.AddScoped(typeof(IWriteDailyFoodLogRepository),typeof(WriteDailyFoodLogRepository));
		services.AddScoped(typeof(IReadUserRepository),typeof(ReadUserRepository));
		services.AddScoped(typeof(IWriteUserRepository),typeof(WriteUserRepository));
		services.AddScoped(typeof(IReadBranchUserRepository),typeof(ReadBranchUserRepository));
		services.AddScoped(typeof(IWriteBranchUserRepository),typeof(WriteBranchUserRepository));
		services.AddScoped(typeof(IReadTransferPersonalRepository),typeof(ReadTransferPersonalRepository));
		services.AddScoped(typeof(IWriteTransferPersonalRepository),typeof(WriteTransferPersonalRepository));
		services.AddScoped(typeof(IReadMissingDayRepository),typeof(ReadMissingDayRepository));
		services.AddScoped(typeof(IWriteMissingDayRepository),typeof(WriteMissingDayRepository));
		services.AddScoped(typeof(IReadPersonalCumulativeRepository),typeof(ReadPersonalCumulativeRepository));
		services.AddScoped(typeof(IWritePersonalCumulativeRepository),typeof(WritePersonalCumulativeRepository));
		services.AddScoped(typeof(IWriteUserLogRepository),typeof(WriteUserLogRepository));
		services.AddScoped(typeof(IReadUserLogRepository),typeof(ReadUserLogRepository));

		//using var serviceProvider = services.BuildServiceProvider();
		//using var scope = serviceProvider.CreateScope();
		//var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
		//dbContext.Database.Migrate(); // Migration'ları uygular

	}
}