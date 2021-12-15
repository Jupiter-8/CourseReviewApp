using CourseReviewApp.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseReviewApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> GetUser(Expression<Func<AppUser, bool>> filter);
        Task<IEnumerable<AppUser>> GetUsers(Expression<Func<AppUser, bool>> filter = null);
        Task ChangeUserStatus(int id, bool status);
        Task DeleteUser(int id, IList<string> userRoles);
        Task AssignModeratorRole(int id);
        Task UnassignModeratorRole(int id);
    }
}
