using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseReviewApp.Services.Classes
{
    public class UserService : BaseService, IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(AppDbContext dbContext, UserManager<AppUser> userManager) : base(dbContext)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> GetUser(Expression<Func<AppUser, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Filter expression is null.");
            AppUser user = await DbContext.Users.FirstOrDefaultAsync(filter);
            if (user != null)
                user.Roles = await _userManager.GetRolesAsync(user);

            return user;
        }

        public async Task<IEnumerable<AppUser>> GetUsers(Expression<Func<AppUser, bool>> filter = null)
        {
            IQueryable<AppUser> users = DbContext.Users.AsQueryable();
            if (filter != null)
                users = users.Where(filter);

            foreach (var user in users)
                user.Roles = await _userManager.GetRolesAsync(user);

            return users;
        }

        public async Task<Role> GetRole(Expression<Func<Role, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Filter expression is null.");

            return await DbContext.Roles.FirstOrDefaultAsync(filter);
        }

        public async Task ChangeUserStatus(int userId, bool status)
        {
            AppUser user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            bool userExists = await DbContext.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                throw new InvalidOperationException($"User with id: {userId} not found.");
            user.IsActive = status;

            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(int userId, IList<string> userRoles)
        {
            if (userRoles.Contains("Course_owner"))
            {
                CourseOwner owner = await DbContext.Users.OfType<CourseOwner>()
                    .Include(co => co.Courses)
                        .ThenInclude(c => c.Reviews)
                            .ThenInclude(r => r.ReviewReports)
                    .Include(co => co.Courses)
                        .ThenInclude(c => c.Reviews)
                            .ThenInclude(r => r.HelpfullReviews)
                    .Include(co => co.Courses)
                        .ThenInclude(c => c.Reviews)
                            .ThenInclude(c => c.OwnerComment)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (owner == null)
                    throw new InvalidOperationException($"User with id: {userId} not found.");
                DbContext.Users.Remove(owner);
            }
            else if (userRoles.Contains("Course_client"))
            {
                CourseClient client = await DbContext.Users.OfType<CourseClient>()
                    .Include(cc => cc.Reviews)
                        .ThenInclude(r => r.ReviewReports)
                    .Include(cc => cc.Reviews)
                        .ThenInclude(r => r.OwnerComment)
                    .Include(cc => cc.HelpfullReviews)
                    .Include(cc => cc.ObservedCourses)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (client == null)
                    throw new InvalidOperationException($"User with id: {userId} not found.");
                DbContext.Users.Remove(client);
            }
            else
                throw new InvalidOperationException("User for deletion must be in the Course_owner or the Course_client role.");

            await DbContext.SaveChangesAsync();
        }

        public async Task AssignModeratorRoleToUser(int userId)
        {
            AppUser user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new InvalidOperationException($"User with id: {userId} not found.");

            await _userManager.AddToRoleAsync(user, "Moderator");
        }

        public async Task UnassignModeratorRoleFromUser(int userId)
        {
            AppUser user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new InvalidOperationException($"User with id: {userId} not found.");

            await _userManager.RemoveFromRoleAsync(user, "Moderator");
        }

        public async Task DisableUserLockout(int userId)
        {
            AppUser user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new InvalidOperationException($"User with id: {userId} not found.");
            user.LockoutEnd = new DateTime(2000, 1, 1);
            user.LockoutMessageSent = false;

            await DbContext.SaveChangesAsync();
        }
    }
}
