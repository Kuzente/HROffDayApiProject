﻿using System.Text;
using System.Text.Json.Serialization;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using Services;
using Services.HangfireFilter;
using UI.Helpers;
using UI.Middlewares;
using UI.test;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Configuration.GetSection("RootPath").Value = builder.Environment.WebRootPath;
var jwtSettings = builder.Configuration.GetSection("JwtOptions");
builder.Services.AddScoped<JwtHelper>();
QuestPDF.Settings.License = LicenseType.Community;
builder.Services.AddDistributedMemoryCache();

JwtBearerEventHandlers.Initialize(builder.Configuration);
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(options =>
   {
	   options.TokenValidationParameters = new TokenValidationParameters
	   {
		   ValidateIssuer = true,
		   ValidateAudience = true,
		   ValidateLifetime = true,
		   ValidateIssuerSigningKey = true,
		   ValidIssuer = jwtSettings["Issuer"],
		   ValidAudience = jwtSettings["Audience"],
		   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
	   };
	   options.Events = JwtBearerEventHandlers.CreateEvents();
	   
   });
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(10);
    options.Cookie.MaxAge = TimeSpan.FromHours(10);
    options.Cookie.HttpOnly = true;
});
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddOData(conf =>
{
    conf.EnableQueryFeatures();
});
//Test DB
var connectionString = builder.Environment.IsDevelopment()
	? builder.Configuration.GetConnectionString("Local")
	: builder.Configuration.GetConnectionString("ProdDbConnection");
builder.Services.AddServiceLayerService(connectionString, connectionString);
builder.Services.AddSingleton<CustomReflectionGetClass>();
//Test DB
var app = builder.Build();
app.UseMiddleware<RequestCultureMiddleware>(); //Kültür middleware
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RoleUpdateMiddleware>(); //Rol Kontrol middleware eğer kullanıcı aktifken değişirse oturum sonlanıyor
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
	Authorization = new[] { new HangfireAuthorizationFilter() }
});

#region PersonalList
app.MapControllerRoute(name: "personalListGet", pattern: "personeller", defaults: new { controller = "Personal", action = "Index" });
app.MapControllerRoute(name: "personalListCreate", pattern: "create-personal", defaults: new { controller = "Personal", action = "AddPersonal" });
app.MapControllerRoute(name: "personalListGetSelect", pattern: "get-select-items", defaults: new { controller = "Personal", action = "GetBranchAndPositions" });
app.MapControllerRoute(name: "downloadPersonelExcelListGet", pattern: "personeller-excel", defaults: new { controller = "Personal", action = "ExportExcel" });
#endregion
#region PassivePersonelList
app.MapControllerRoute(name: "passivePersonalListGet", pattern: "cikarilan-personeller", defaults: new { controller = "PassivePersonal", action = "Index" });
app.MapControllerRoute(name: "downloadPassivePersonelExcelListGet", pattern: "cikarilan-personeller-excel", defaults: new { controller = "PassivePersonal", action = "ExportExcel" });

#endregion
#region PersonalDetails
app.MapControllerRoute(name: "personalDetailPage", pattern: "get-personel-detaylari", defaults: new { controller = "PersonalDetail", action = "EditAjax" });
app.MapControllerRoute(name: "personalDetailPage", pattern: "personel-detaylari", defaults: new { controller = "PersonalDetail", action = "Edit" });
app.MapControllerRoute(name: "personalDetailPageStatusPost", pattern: "personel-durumu", defaults: new { controller = "PersonalDetail", action = "ChangeStatus" });
app.MapControllerRoute(name: "personalDeletePost", pattern: "personel-sil", defaults: new { controller = "PersonalDetail", action = "ArchivePersonal" });
app.MapControllerRoute(name: "personalOffDays", pattern: "personel-izinleri", defaults: new { controller = "PersonalDetail", action = "PersonalOffDayList" });
app.MapControllerRoute(name: "personalHeader", pattern: "personel-header", defaults: new { controller = "PersonalDetail", action = "PersonelDetailsHeader" });
app.MapControllerRoute(name: "personalTransferListPage", pattern: "personel-nakil-listesi", defaults: new { controller = "PersonalDetail", action = "PersonalTransferList" });
app.MapControllerRoute(name: "personalTransferDelete", pattern: "personel-nakil-sil", defaults: new { controller = "PersonalDetail", action = "PersonalTransferDelete" });
app.MapControllerRoute(name: "personalTransferExcel", pattern: "personel-nakil-excel", defaults: new { controller = "PersonalDetail", action = "PersonalTransferExportExcel" });
app.MapControllerRoute(name: "personalMissingDayPage", pattern: "personel-eksik-gun-listesi", defaults: new { controller = "PersonalDetail", action = "PersonalMissingDayList" });
app.MapControllerRoute(name: "personalMissingDayDelete", pattern: "personel-eksik-gun-sil", defaults: new { controller = "PersonalDetail", action = "PersonalMissingDayDelete" });
app.MapControllerRoute(name: "personalMissingDayAdd", pattern: "personel-eksik-gun-ekle", defaults: new { controller = "PersonalDetail", action = "PersonalMissingDayAdd" });
app.MapControllerRoute(name: "personalMissingDayExcel", pattern: "personel-eksik-gun-excel", defaults: new { controller = "PersonalDetail", action = "PersonalMissingDayExportExcel" });
app.MapControllerRoute(name: "personalDetailsCumulativeAddOrUpdatePost", pattern: "personel-detaylari-kumulatif-guncelle", defaults: new { controller = "PersonalDetail", action = "UpdateCumulative" });
#endregion
#region BranchList
app.MapControllerRoute(name: "branchListGet", pattern: "subeler", defaults: new { controller = "Branch", action = "Index" });
app.MapControllerRoute(name: "downloadBranchExcelListGet", pattern: "subeler-excel", defaults: new { controller = "Branch", action = "ExportExcel" });
app.MapControllerRoute(name: "updateBranchGetPost", pattern: "sube-duzenle", defaults: new { controller = "Branch", action = "UpdateBranch" });
app.MapControllerRoute(name: "addBranchPost", pattern: "sube-ekle", defaults: new { controller = "Branch", action = "AddBranch" });
app.MapControllerRoute(name: "deleteBranchPost", pattern: "sube-sil", defaults: new { controller = "Branch", action = "ArchiveBranch" });

#endregion
#region PositionList
app.MapControllerRoute(name: "positionListGet", pattern: "unvanlar", defaults: new { controller = "Position", action = "Index" });
app.MapControllerRoute(name: "downloadPositionExcelListGet", pattern: "unvanlar-excel", defaults: new { controller = "Position", action = "ExportExcel" });
app.MapControllerRoute(name: "updatePositionGetPost", pattern: "unvan-duzenle", defaults: new { controller = "Position", action = "UpdatePosition" });
app.MapControllerRoute(name: "addPositionPost", pattern: "unvan-ekle", defaults: new { controller = "Position", action = "AddPosition" });
app.MapControllerRoute(name: "deletePositionPost", pattern: "unvan-sil", defaults: new { controller = "Position", action = "ArchivePosition" });

#endregion
#region OffDay
app.MapControllerRoute(name: "offDayCreateGet", pattern: "izin-olustur", defaults: new { controller = "OffDay", action = "AddOffDay" });
app.MapControllerRoute(name: "offDayWaitingList", pattern: "bekleyen-izinler", defaults: new { controller = "OffDay", action = "WaitingOffDayList" });
app.MapControllerRoute(name: "offDayFirstWaitingChangeStatus", pattern: "bekleyen-izin-guncelle-bir", defaults: new { controller = "OffDay", action = "UpdateFirstWaitingStatus" });
app.MapControllerRoute(name: "offDaySecondWaitingChangeStatus", pattern: "bekleyen-izin-guncelle-iki", defaults: new { controller = "OffDay", action = "UpdateSecondWaitingStatus" });
app.MapControllerRoute(name: "offDayWaitingEdit", pattern: "izin-duzenle", defaults: new { controller = "OffDay", action = "WaitingOffDayEdit" });
app.MapControllerRoute(name: "offDayRejectedList", pattern: "reddedilen-izinler", defaults: new { controller = "OffDay", action = "RejectedOffDayList" });
app.MapControllerRoute(name: "offDayApprovedList", pattern: "onaylanan-izinler", defaults: new { controller = "OffDay", action = "ApprovedOffDayList" });
app.MapControllerRoute(name: "offDayWaitingList", pattern: "izin-sil", defaults: new { controller = "OffDay", action = "DeleteOffDay" });
app.MapControllerRoute(name: "offDayExcel", pattern: "izin-excel", defaults: new { controller = "OffDay", action = "ExportExcel" });
app.MapControllerRoute(name: "offDayPdf", pattern: "/create-pdf", defaults: new { controller = "OffDay", action = "ExportPdf" });

#endregion
#region RecoveryPages
app.MapControllerRoute(name: "recoveryBranchList", pattern: "silinen-subeler", defaults: new { controller = "Recovery", action = "DeletedBranch" });
app.MapControllerRoute(name: "recoveryPositionList", pattern: "silinen-unvanlar", defaults: new { controller = "Recovery", action = "DeletedPosition" });
app.MapControllerRoute(name: "recoveryPersonalList", pattern: "silinen-personeller", defaults: new { controller = "Recovery", action = "DeletedPersonal" });
app.MapControllerRoute(name: "recoveryOffDayList", pattern: "silinen-izinler", defaults: new { controller = "Recovery", action = "DeletedOffDay" });
app.MapControllerRoute(name: "recoveryBranch", pattern: "sube-gerigetir", defaults: new { controller = "Recovery", action = "RecoverBranch" });
app.MapControllerRoute(name: "recoveryPosition", pattern: "unvan-gerigetir", defaults: new { controller = "Recovery", action = "RecoverPosition" });
app.MapControllerRoute(name: "recoveryPersonal", pattern: "personel-gerigetir", defaults: new { controller = "Recovery", action = "RecoverPersonal" });


#endregion
#region MultipleUpload
app.MapControllerRoute(name: "downloadPersonalScheme", pattern: "download-personal-scheme", defaults: new { controller = "MultipleUpload", action = "GetExcelUploadSheme" });
app.MapControllerRoute(name: "downloadSalaryExcelScheme", pattern: "download-salary-scheme", defaults: new { controller = "MultipleUpload", action = "GetExcelSalaryUploadSheme" });
app.MapControllerRoute(name: "downloadIbanExcelScheme", pattern: "download-iban-scheme", defaults: new { controller = "MultipleUpload", action = "GetExcelIBANUploadSheme" });
app.MapControllerRoute(name: "downloadBankAccountExcelScheme", pattern: "download-bank-account-scheme", defaults: new { controller = "MultipleUpload", action = "GetExcelBankAccountUploadSheme" });
app.MapControllerRoute(name: "multipleUploadGET", pattern: "toplu-islemler", defaults: new { controller = "MultipleUpload", action = "PersonalUpload" });
app.MapControllerRoute(name: "personalUploadPOST", pattern: "toplu-personel-yukle", defaults: new { controller = "MultipleUpload", action = "PersonalUpload" });
app.MapControllerRoute(name: "salaryUploadPOST", pattern: "toplu-maas-yukle", defaults: new { controller = "MultipleUpload", action = "SalaryUpload" });
app.MapControllerRoute(name: "ibanUploadPOST", pattern: "toplu-iban-yukle", defaults: new { controller = "MultipleUpload", action = "IbanUpload" });
app.MapControllerRoute(name: "bankAccountUploadPOST", pattern: "toplu-bankno-yukle", defaults: new { controller = "MultipleUpload", action = "BankAccountUpload" });
#endregion
#region DailyLog
app.MapControllerRoute(name: "dailyLogPage", pattern: "gunluk-takip", defaults: new { controller = "DailyLog", action = "Index" });
app.MapControllerRoute(name: "dailyYearLog", pattern: "yillik-izin-log", defaults: new { controller = "DailyLog", action = "GetYearLogs" });
app.MapControllerRoute(name: "dailyFoodLog", pattern: "gida-yardimi-log", defaults: new { controller = "DailyLog", action = "GetFoodLogs" });
#endregion
#region User
app.MapControllerRoute(name: "userListPage", pattern: "kullanici-listesi", defaults: new { controller = "User", action = "UsersList" });
app.MapControllerRoute(name: "createUser", pattern: "kullanici-ekle", defaults: new { controller = "User", action = "CreateUser" });
app.MapControllerRoute(name: "updateUser", pattern: "kullanici-guncelle", defaults: new { controller = "User", action = "UpdateUser" });
app.MapControllerRoute(name: "deleteUser", pattern: "kullanici-sil", defaults: new { controller = "User", action = "DeleteUser" });
app.MapControllerRoute(name: "getDirectorBranchSelect", pattern: "select-director-branch", defaults: new { controller = "User", action = "GetDirectorSelects" });
app.MapControllerRoute(name: "getDirectorBranchSelect", pattern: "select-branchmanager-branch", defaults: new { controller = "User", action = "GetBranchManagerSelects" });


#endregion
#region Authentication
app.MapControllerRoute(name: "loginPage", pattern: "giris-yap", defaults: new { controller = "Authentication", action = "Login" });
app.MapControllerRoute(name: "logout", pattern: "cikis-yap", defaults: new { controller = "Authentication", action = "Logout" });
app.MapControllerRoute(name: "newPasswordPage", pattern: "yeni-parola", defaults: new { controller = "Authentication", action = "NewPassword" });
app.MapControllerRoute(name: "newPasswordPage", pattern: "parola-sifirla", defaults: new { controller = "Authentication", action = "ResetPassword" });
app.MapControllerRoute(name: "forgotPasswordPage", pattern: "parolami-unuttum", defaults: new { controller = "Authentication", action = "ForgotPassword" });
app.MapControllerRoute(name: "forgotPasswordPost", pattern: "parola-sifirlama-baglantisi-gonder", defaults: new { controller = "Authentication", action = "ForgotPasswordPost" });



#endregion

#region Dashboard
app.MapControllerRoute(name: "postCumulativeNotification", pattern: "post-kumulatif-bildirim", defaults: new { controller = "Home", action = "PostCumulativeNotification" });


#endregion
#region UsersLog
app.MapControllerRoute(name: "usersLogsListPage", pattern: "kullanici-hareketleri", defaults: new { controller = "UserLog", action = "UsersLogList" });

#endregion
#region TransferPersonal
app.MapControllerRoute(name: "transferPersonalListPage", pattern: "gorevlendirmeler", defaults: new { controller = "TransferPersonal", action = "TransferPersonalList" });
app.MapControllerRoute(name: "downloadTransferPersonalExcelListGet", pattern: "gorevlendirmeler-excel", defaults: new { controller = "TransferPersonal", action = "ExportExcel" });
#endregion
#region MissingDay
app.MapControllerRoute(name: "missingDayListPage", pattern: "eksik-gunler", defaults: new { controller = "MissingDay", action = "MissingDayList" });
app.MapControllerRoute(name: "downloadMissingDayListExcelListGet", pattern: "eksik-gunler-excel", defaults: new { controller = "MissingDay", action = "ExportExcel" });
#endregion
#region NormKadro
app.MapControllerRoute(name: "downloadDepartmentExcelList", pattern: "norm-kadro", defaults: new { controller = "Branch", action = "ExportExcelDepartment" });
#endregion
#region DetailedFilter
app.MapControllerRoute(name: "detailedFilterPage", pattern: "detayli-filtre", defaults: new { controller = "DetailedFilter", action = "Index" });
#endregion
#region ErrorPages
app.MapControllerRoute(name: "error404page", pattern: "404", defaults: new { controller = "Home", action = "ErrorPage" });
app.MapControllerRoute(name: "accessDeniedpage", pattern: "403", defaults: new { controller = "Home", action = "AccessDeniedPage" });


#endregion
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
