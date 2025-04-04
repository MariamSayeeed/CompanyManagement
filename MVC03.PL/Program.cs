using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC03.BLL.Interfaces;
using MVC03.BLL.Repositories;
using MVC03.DAL.Data.Contexts;
using MVC03.DAL.Models;
using MVC03.PL.Helpers;
using MVC03.PL.Mapping;
using MVC03.PL.Settings;
using System.Configuration;

namespace MVC03.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow Dependency Injection for DepartmentRepository 
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // Allow Dependency Injection for EmployeeRepository 

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>()
                            .AddDefaultTokenProviders();  // Enable Token
           
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow Dependency Injection for CompanyDbContext

            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddAutoMapper(M=> M.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(M=> M.AddProfile(new DepartmentProfile()));

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = ("/Account/SignIn");
                config.LogoutPath = ("/Home/SignIn");
                config.AccessDeniedPath = ("/Account/AccessDenied");

            });

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
            builder.Services.AddScoped<IMailService, MailService>();

            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection(nameof(TwilioSettings)));
            builder.Services.AddScoped<ITwilioService, TwilioService>();



            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //})
            // .AddCookie
            // (
            //    options =>
            //    {
            //        options.LoginPath = "/Account/SignIn";
            //        options.AccessDeniedPath = "/Account/AccessDenied";
            //        options.Cookie.SameSite = SameSiteMode.None;
            //        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //    })
            // .AddGoogle(options =>
            // {
            //     IConfiguration AuthPath = builder.Configuration.GetSection("Authentication:Google");
            //     options.ClientId = AuthPath["ClientId"];
            //     options.ClientSecret = AuthPath["ClientSecret"];
            //    // options.CallbackPath = "/signin-google";
            // });

            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/Account/SignIn";
            //    options.AccessDeniedPath = "/Account/AccessDenied";
            //    options.Cookie.SameSite = SameSiteMode.None;
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //});




            ////- -------------------------------   Build --------
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
        }
    }
}
