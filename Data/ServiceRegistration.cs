using Data.Abstract;
using Data.Abstract.BranchRepositories;
using Data.Abstract.OffDayRepositories;
using Data.Abstract.PersonalRepositories;
using Data.Abstract.PositionRepositories;
using Data.Concrete;
using Data.Concrete.BranchRepositories;
using Data.Concrete.OffDayRepositories;
using Data.Concrete.PersonalRepositories;
using Data.Concrete.PositionRepositories;
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

	}
}