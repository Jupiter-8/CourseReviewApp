using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Classes;
using CourseReviewApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace CourseReviewApp.Tests
{
    public class Startup
    {
        public Startup()
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(opt =>
                   opt.UseInMemoryDatabase(new Guid().ToString()));

            services.AddIdentityCore<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
            })
                    .AddRoles<Role>()
                    .AddRoleManager<RoleManager<Role>>()
                    .AddUserManager<UserManager<AppUser>>()
                    .AddEntityFrameworkStores<AppDbContext>();

            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IReportService, ReportService>();

            services.AddSingleton<IConfiguration>(x =>
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
                return builder.Build();
            });
        }
    }
}
