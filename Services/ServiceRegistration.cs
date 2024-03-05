using Data;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstract.BranchServices;
using Services.Abstract.PersonalServices;
using Services.Abstract.PositionServices;
using Services.Concrete.BranchServices;
using Services.Concrete.PersonalServices;
using Services.Concrete.PositionServices;
using System.Reflection;
using Hangfire;
using Services.Abstract.DailyCounterServices;
using Services.Abstract.DashboardServices;
using Services.Abstract.OffDayServices;
using Services.Concrete.DailyCounterServices;
using Services.Concrete.DashboardServices;
using Services.Concrete.OffDayServices;
using Services.ExcelDownloadServices;
using Services.ExcelDownloadServices.BranchServices;
using Services.ExcelDownloadServices.OffDayServices;
using Services.ExcelDownloadServices.PersonalServices;
using Services.ExcelDownloadServices.PositionServices;
using Services.FileUpload;
using Services.Hangfire;
using Services.PdfDownloadServices;
using IDocument = QuestPDF.Infrastructure.IDocument;

namespace Services;

public static class ServiceRegistration
{
	public static void AddServiceLayerService(this IServiceCollection services , string? connectionstring, string? hangfireConnectionstring) 
	{ 
		var assembly = Assembly.GetExecutingAssembly();
		services.AddAutoMapper(assembly);
		services.AddDataLayerService(connectionstring);
		services.AddScoped(typeof(IReadPersonalService), typeof(ReadPersonalService));
		services.AddScoped(typeof(IWritePersonalService), typeof(WritePersonalService));
		services.AddScoped(typeof(IReadBranchService), typeof(ReadBranchService));
		services.AddScoped(typeof(IWriteBranchService), typeof(WriteBranchService));
		services.AddScoped(typeof(IReadPositionService), typeof(ReadPositionService));
		services.AddScoped(typeof(IWritePositionService), typeof(WritePositionService));services.AddScoped(typeof(IReadPositionService), typeof(ReadPositionService));
		services.AddScoped(typeof(IReadOffDayService), typeof(ReadOffDayService));
		services.AddScoped(typeof(IWriteOffDayService), typeof(WriteOffDayService));
		services.AddScoped(typeof(ExcelPersonalAddrange));
		services.AddScoped(typeof(PersonalExcelExport));
		services.AddScoped(typeof(PassivePersonalExcelExport));
		services.AddScoped(typeof(BranchExcelExport));
		services.AddScoped(typeof(PositionExcelExport));
		services.AddScoped(typeof(OffDayExcelExport));
		services.AddScoped(typeof(ExcelUploadScheme));
		services.AddScoped(typeof(DailyJob));
		services.AddScoped(typeof(IReadOdataService), typeof(ReadOdataService));
		services.AddScoped(typeof(IReadDailyCounterService), typeof(ReadDailyCounterService));
		services.AddScoped(typeof(IWriteDailyCounterService), typeof(WriteDailyCounterService));
		services.AddScoped( typeof(OffDayFormPdf));
		services.AddHangfire(x =>
		{
			x.UseSqlServerStorage(hangfireConnectionstring);
			RecurringJob.AddOrUpdate<WriteDailyCounterService>("YıllıkIzinYeniJob", j => j.AddDailyCounterLogService(), "36 1 * * *", options: new RecurringJobOptions
			{
				TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"),
				
			});
			
		});
		services.AddHangfireServer();
	}
}