using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CourseReviewApp.Tests
{
    public static class SeedExtension
    {
        public static async void SeedData(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            var courseOwnerRole = new Role()
            {
                Id = 1,
                Name = "Course_owner",
                RoleValue = RoleValue.Course_owner
            };

            var courseClientRole = new Role()
            {
                Id = 2,
                Name = "Course_client",
                RoleValue = RoleValue.Course_client
            };

            var moderatorRole = new Role()
            {
                Id = 3,
                Name = "Moderator",
                RoleValue = RoleValue.Moderator
            };

            var adminRole = new Role()
            {
                Id = 4,
                Name = "Admin",
                RoleValue = RoleValue.Admin
            };

            await roleManager.CreateAsync(courseClientRole);
            await roleManager.CreateAsync(courseOwnerRole);
            await roleManager.CreateAsync(moderatorRole);
            await roleManager.CreateAsync(adminRole);

            var category = new Category()
            {
                Id = 1,
                Name = "Programming",
                ParentCategoryId = null
            };

            await dbContext.Categories.AddRangeAsync(category);

            await dbContext.SaveChangesAsync();
        }
    }
}
