using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.Repository;
using HalloDoc.Repository.IRepository;
using HalloDoc.Hubs;
using Microsoft.EntityFrameworkCore;
using HalloDoc.Services.IServices;
using HalloDoc.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationDbContext")), ServiceLifetime.Scoped);
builder.Services.AddScoped<IAspNetUserRepository, AspNetUserRepository>();
builder.Services.AddScoped<IRequestClientRepository, RequestClientRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IRequestWiseFilesRepository, RequestWiseFilesRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddScoped<IConciergeRepository, ConciergeRepository>();
builder.Services.AddScoped<IPhysicianRepository, PhysicianRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IRequestNotesRepository, RequestNotesRepository>();
builder.Services.AddScoped<ICaseTagRepository,CaseTagRepository>();
builder.Services.AddScoped<IRequestStatusLogRepository, RequestStatusLogRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IHealthProfessionalTypeRepository, HealthProfessionalTypeRepository>();
builder.Services.AddScoped<IHealthProfessionalsRepository,HealthProfessionalsRepository>();
builder.Services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();
builder.Services.AddScoped<IBlockRequestRepository, BlockRequestRepository>();
builder.Services.AddScoped<IAspNetUserRolesRepository, AspNetUserRolesRepository>();
builder.Services.AddScoped<IEncounterRepository, EncounterRepository>();
builder.Services.AddScoped<IRequestClosedRepository, RequestClosedRepository>();
builder.Services.AddScoped<IAdminRegionRepository, AdminRegionRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPhysicianNotificationRepository,PhysicianNotificationRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IRoleMenuRepository, RoleMenuRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IEmailLogsRepository, EmailLogsRepository>();
builder.Services.AddScoped<ISMSLogRepository,SMSLogRepository>();
builder.Services.AddScoped<IShiftDetailsRepository, ShiftDetailsRepository>();
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<IPhysicianLocationRepository, PhysicianLocationRepository>();
builder.Services.AddScoped<IPhysicianRegionRepository, PhysicianRegionRepository>();
builder.Services.AddScoped<IQuaterSheetRepository , QuaterSheetRepository>();
builder.Services.AddScoped<ITimeDetailsRepository, TimeDetailsRepository>();
builder.Services.AddScoped<IPayRateRepository, PayRateRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IChatRepository,_ChatRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IProviderService, ProviderService>();    
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICustomService, CustomService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IOTimeout = TimeSpan.FromSeconds(5);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
builder.Services.AddSignalR();

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
app.UseSession();
app.MapControllerRoute(
    name: "default",
     
//   pattern: "{controller=Custom}/{action=PatientSite}/{id?}");
pattern: "{controller=Custom}/{action=AdminLogin}/{id?}");
app.MapHub<ChatHub>("/chatHub");

app.Run();
