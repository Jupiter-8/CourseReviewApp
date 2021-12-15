﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
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

        public UserService(AppDbContext dbContext, ILogger logger, IMapper mapper, UserManager<AppUser> userManager)
            : base(dbContext, logger, mapper)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> GetUser(Expression<Func<AppUser, bool>> filter)
        {
            try
            {
                if (filter == null)
                    throw new ArgumentNullException("Filter expression is null.");
                AppUser user = await DbContext.Users.FirstOrDefaultAsync(filter);
                if (user != null)
                    user.Roles = await _userManager.GetRolesAsync(user);

                return user;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AppUser>> GetUsers(Expression<Func<AppUser, bool>> filter = null)
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ChangeUserStatus(int id, bool status)
        {
            try
            {
                AppUser user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                bool userExists = await DbContext.Users.AnyAsync(u => u.Id == id);
                if (!userExists)
                    throw new KeyNotFoundException($"User with id: {id} not found.");
                user.IsActive = status;

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteUser(int id, IList<string> userRoles)
        {
            try
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
                    throw new ArgumentException("Deleted user must be in the Course_owner or the Course_client role.");

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task AssignModeratorRole(int id)
        {
            try
            {
                AppUser user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                bool userExists = await DbContext.Users.AnyAsync(u => u.Id == id);
                if (!userExists)
                    throw new KeyNotFoundException($"User with id: {id} not found.");

                await _userManager.AddToRoleAsync(user, "Moderator");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UnassignModeratorRole(int id)
        {
            try
            {
                AppUser user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                bool userExists = await DbContext.Users.AnyAsync(u => u.Id == id);
                if (!userExists)
                    throw new KeyNotFoundException($"User with id: {id} not found.");

                await _userManager.RemoveFromRoleAsync(user, "Moderator");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}