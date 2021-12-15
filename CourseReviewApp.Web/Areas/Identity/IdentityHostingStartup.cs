using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;

[assembly: HostingStartup(typeof(CourseReviewApp.Web.Areas.Identity.IdentityHostingStartup))]
namespace CourseReviewApp.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}