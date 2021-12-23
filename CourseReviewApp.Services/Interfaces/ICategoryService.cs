using CourseReviewApp.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseReviewApp.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> GetCategory(Expression<Func<Category, bool>> filter);
        IEnumerable<Category> GetCategories(Expression<Func<Category, bool>> filter = null);
        Task AddOrEditCategory(Category category);
        Task DeleteCategory(int categoryId);
    }
}
