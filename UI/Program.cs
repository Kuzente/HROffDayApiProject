using System.Text.Json.Serialization;
using Microsoft.AspNetCore.OData;
using Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddOData(conf =>
{
    conf.EnableQueryFeatures();
});
builder.Services.AddServiceLayerService(builder.Configuration.GetConnectionString("Mssql"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

#region PersonalDetails
app.MapControllerRoute(name: "personalDetailPage", pattern: "get-personel-detayları", defaults: new { controller = "PersonalDetail", action = "EditAjax" });
app.MapControllerRoute(name: "personalDetailPage", pattern: "personel-detayları", defaults: new { controller = "PersonalDetail", action = "Edit" });
app.MapControllerRoute(name: "personalDetailPageStatusPost", pattern: "personel-durumu", defaults: new { controller = "PersonalDetail", action = "ChangeStatus" });
app.MapControllerRoute(name: "personalDeletePost", pattern: "personel-sil", defaults: new { controller = "PersonalDetail", action = "ArchivePersonal" });
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

#region RecoveryPages
app.MapControllerRoute(name: "recoveryBranchList", pattern: "silinen-subeler", defaults: new { controller = "Recovery", action = "DeletedBranch" });
app.MapControllerRoute(name: "recoveryPositionList", pattern: "silinen-unvanlar", defaults: new { controller = "Recovery", action = "DeletedPosition" });
app.MapControllerRoute(name: "recoveryPersonalList", pattern: "silinen-personeller", defaults: new { controller = "Recovery", action = "DeletedPersonal" });
app.MapControllerRoute(name: "recoveryBranch", pattern: "sube-gerigetir", defaults: new { controller = "Recovery", action = "RecoverBranch" });
app.MapControllerRoute(name: "recoveryPosition", pattern: "unvan-gerigetir", defaults: new { controller = "Recovery", action = "RecoverPosition" });
app.MapControllerRoute(name: "recoveryPersonal", pattern: "personel-gerigetir", defaults: new { controller = "Recovery", action = "RecoverPersonal" });


#endregion
#region MultipleUpload
app.MapControllerRoute(name: "downloadScheme", pattern: "download-scheme", defaults: new { controller = "MultipleUpload", action = "GetExcelSheme" });
app.MapControllerRoute(name: "personalUpload", pattern: "personal-upload", defaults: new { controller = "MultipleUpload", action = "PersonalUpload" });


#endregion
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
