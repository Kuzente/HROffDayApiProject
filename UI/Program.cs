using System.Text.Json.Serialization;
using Data.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using NToastNotify;
using Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = true,
    PositionClass = ToastPositions.TopRight,
    
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.LoginPath = "/giris-yap";
        options.Cookie.Name = "user";

    });
// builder.Services.AddIdentity<IdentityUser, IdentityRole>(p=>
// {
//     p.Password.RequiredUniqueChars = 0;
//     p.Password.RequireUppercase = false;
//     p.Password.RequireNonAlphanumeric = false;
// }).AddEntityFrameworkStores<DataContext>();
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddOData(conf =>
{
    conf.EnableQueryFeatures();
});
builder.Services.AddServiceLayerService(builder.Configuration.GetConnectionString("MssqlSomee"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseNToastNotify();
app.UseAuthorization();

#region Dashboard



#endregion
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
app.MapControllerRoute(name: "personalDetailPage", pattern: "get-personel-detayları", defaults: new { controller = "PersonalDetail", action = "EditAjax" });
app.MapControllerRoute(name: "personalDetailPage", pattern: "personel-detayları", defaults: new { controller = "PersonalDetail", action = "Edit" });
app.MapControllerRoute(name: "personalDetailPageStatusPost", pattern: "personel-durumu", defaults: new { controller = "PersonalDetail", action = "ChangeStatus" });
app.MapControllerRoute(name: "personalDeletePost", pattern: "personel-sil", defaults: new { controller = "PersonalDetail", action = "ArchivePersonal" });
app.MapControllerRoute(name: "personalOffDays", pattern: "personel-izinleri", defaults: new { controller = "PersonalDetail", action = "PersonalOffDayList" });
app.MapControllerRoute(name: "personalHeader", pattern: "personel-header", defaults: new { controller = "PersonalDetail", action = "PersonelDetailsHeader" });
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
app.MapControllerRoute(name: "downloadScheme", pattern: "download-scheme", defaults: new { controller = "MultipleUpload", action = "GetExcelSheme" });
app.MapControllerRoute(name: "personalUpload", pattern: "toplu-islemler", defaults: new { controller = "MultipleUpload", action = "PersonalUpload" });


#endregion

#region QRPages
app.MapControllerRoute(name: "qrListGet", pattern: "qr-islemleri", defaults: new { controller = "Qr", action = "QRList" });

#endregion

#region Authentication
app.MapControllerRoute(name: "loginPage", pattern: "giris-yap", defaults: new { controller = "Authentication", action = "Login" });
app.MapControllerRoute(name: "loginPage", pattern: "cikis-yap", defaults: new { controller = "Authentication", action = "Logout" });
app.MapControllerRoute(name: "loginPage", pattern: "/create-pdf", defaults: new { controller = "OffDay", action = "createpdf" });


#endregion
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
