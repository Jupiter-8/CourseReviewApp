using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseReviewApp.Services.Classes
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Category> GetCategory(Expression<Func<Category, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Filter expression is null.");

            return await DbContext.Categories.FirstOrDefaultAsync(filter);
        }

        public IEnumerable<Category> GetCategories(Expression<Func<Category, bool>> filter = null)
        {
            IQueryable<Category> categories = DbContext.Categories.AsQueryable();
            if (filter != null)
                categories = categories.Where(filter);

            return categories;
        }

        public async Task AddOrEditCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("Model is null.");
            if (category.ParentCategoryId.HasValue)
            {
                bool parentCategoryExists = await DbContext.Categories.AnyAsync(c => c.Id == category.ParentCategoryId);
                if (!parentCategoryExists)
                    throw new InvalidOperationException($"Category with id: {category.ParentCategoryId} not found.");
            }
            if (category.Id != 0)
                DbContext.Categories.Update(category);
            else
                await DbContext.Categories.AddAsync(category);

            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteCategory(int categoryId)
        {
            Category category = await DbContext.Categories
                    .Include(c => c.SubCategories)
                    .FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
                throw new InvalidOperationException($"Category with id: {categoryId} not found.");
            DbContext.Categories.Remove(category);

            await DbContext.SaveChangesAsync();
        }
    }
}
