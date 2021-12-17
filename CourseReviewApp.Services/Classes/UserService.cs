﻿using CourseReviewApp.DAL.EntityFramework;
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
            {
                user.Roles = await _userManager.GetRolesAsync(user);
            }

            return users;
        }

        public async Task<Role> GetRole(Expression<Func<Role, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Filter expression is null.");
            Role role = await DbContext.Roles.FirstOrDefaultAsync(filter);

            return role;
        }

        public async Task ChangeUserStatus(int id, bool status)
        {
            AppUser user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            bool userExists = await DbContext.Users.AnyAsync(u => u.Id == id);
            if (!userExists)
                throw new KeyNotFoundException($"User with id: {id} not found.");
            user.IsActive = status;

            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(int id, IList<string> userRoles)
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
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (owner == null)
                    throw new KeyNotFoundException($"User with id: {id} not found.");
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
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (client == null)
                    throw new KeyNotFoundException($"User with id: {id} not found.");
                DbContext.Users.Remove(client);
            }
            else
                throw new ArgumentException("User for deletion must be in the Course_owner or the Course_client role.");

            await DbContext.SaveChangesAsync();
        }

        public async Task AssignModeratorRole(int id)
        {
            AppUser user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            bool userExists = await DbContext.Users.AnyAsync(u => u.Id == id);
            if (!userExists)
                throw new KeyNotFoundException($"User with id: {id} not found.");

            await _userManager.AddToRoleAsync(user, "Moderator");
        }

        public async Task UnassignModeratorRole(int id)
        {
            AppUser user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            bool userExists = await DbContext.Users.AnyAsync(u => u.Id == id);
            if (!userExists)
                throw new KeyNotFoundException($"User with id: {id} not found.");

            await _userManager.RemoveFromRoleAsync(user, "Moderator");
        }
    }
}
