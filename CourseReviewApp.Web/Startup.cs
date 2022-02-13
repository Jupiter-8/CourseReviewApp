using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Classes;
using CourseReviewApp.Services.Interfaces;
using CourseReviewApp.Web.Services.Classes;
using CourseReviewApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;

namespace CourseReviewApp.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<AppUser>(options =>
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

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Error/403";
            });

            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IFileService, FileService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddTransient(typeof(ILogger), typeof(Logger<Startup>));
            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();

            services.AddScoped((serviceProvider) =>
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory factory)
        {
            factory.AddFile("Logs/log.txt");
            if (env.IsDevelopment())
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}