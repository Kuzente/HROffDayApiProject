using Data;
using Data.Abstract.BranchRepositories;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstract.BranchServices;
using Services.Abstract.PersonalServices;
using Services.Concrete.BranchServices;
using Services.Concrete.PersonalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public static class ServiceRegistiration
	{
		public static void AddServiceLayerService(this IServiceCollection services , string connectionstring) 
		{ 
			var assembly = Assembly.GetExecutingAssembly();
			services.AddAutoMapper(assembly);
			services.AddDataLayerService(connectionstring);
			services.AddScoped(typeof(IReadPersonalService), typeof(ReadPersonalService));
			services.AddScoped(typeof(IWritePersonalService), typeof(WritePersonalService));
			services.AddScoped(typeof(IReadBranchService), typeof(ReadBranchService));
			services.AddScoped(typeof(IWriteBranchService), typeof(WriteBranchService));
		}
	}
}
