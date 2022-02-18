using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Classes;
using CourseReviewApp.Services.Interfaces;
using CourseReviewApp.Web.Services.Classes;
using CourseReviewApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

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
                   opt.UseInMemoryDatabase("InMemoryDatabase"));

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
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();

            services.AddSingleton<IConfiguration>(x =>
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
                return builder.Build();
            });

            services.AddTransient((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                return new SmtpClient()
                {
                    Host = config.GetValue<string>("Email:Smtp:Host"),
                    Port = config.GetValue<int>("Email:Smtp:Port"),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                            config.GetValue<string>("Email:Smtp:Username"), config.GetValue<string>("Email:Smtp:Password"))
                };
            });

            services.SeedData();
        }
    }
}
