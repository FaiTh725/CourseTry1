using CourseTry1.Dal;
using CourseTry1.Dal.Interfaces;
using CourseTry1.Dal.Repositories;
using CourseTry1.Domain.Entity;
using CourseTry1.Service;
using CourseTry1.Service.Implementations;
using CourseTry1.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options => 
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IHomeService, HomeService>();
builder.Services.AddTransient<IAccountRepository<User>, AccountRepository>();
builder.Services.AddTransient<IFileRepository, FileRepository>();
builder.Services.AddTransient<IExcelFileRepository, ExcelFileRepository>();
builder.Services.AddTransient<IGroupRepository, GroupRepository>();
builder.Services.AddTransient<IProfileRepository, ProfileRepository>();

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


app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
