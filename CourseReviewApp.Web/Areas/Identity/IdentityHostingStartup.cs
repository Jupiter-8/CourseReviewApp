using Microsoft.AspNetCore.Hosting;

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