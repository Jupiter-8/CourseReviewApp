using AutoMapper;
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
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(AppDbContext dbContext, ILogger logger, IMapper mapper)
            : base(dbContext, logger, mapper)
        {
        }

        public async Task<Category> GetCategory(Expression<Func<Category, bool>> filter)
        {
            try
            {
                if (filter == null)
                    throw new ArgumentNullException("Filter expression is null.");
                Category category = await DbContext.Categories.FirstOrDefaultAsync(filter);

                return category;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<Category> GetCategories(Expression<Func<Category, bool>> filter = null)
        {
            try
            {
                IQueryable<Category> categories = DbContext.Categories.AsQueryable();
                if (filter != null)
                    categories = categories.Where(filter);

                return categories;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task AddOrEditCategory(Category category)
        {
            try
            {
                if (category == null)
                    throw new ArgumentNullException("Model is null.");
                if (category.ParentCategoryId.HasValue)
                {
                    bool parentCategoryExists = await DbContext.Categories.AnyAsync(c => c.Id == category.ParentCategoryId);
                    if (!parentCategoryExists)
                        throw new KeyNotFoundException($"Category with id: {category.ParentCategoryId} not found.");
                }

                if (category.Id != 0)
                    DbContext.Categories.Update(category);
                else
                    await DbContext.Categories.AddAsync(category);

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteCategory(int id)
        {
            try
            {
                Category category = await DbContext.Categories
                    .Include(c => c.SubCategories)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (category == null)
                    throw new KeyNotFoundException($"Category with id: {id} not found.");
                DbContext.Categories.Remove(category);

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
