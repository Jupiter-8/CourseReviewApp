using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CourseReviewApp.Tests.UnitTests
{
    public class CategoryServiceUnitTests : BaseUnitTests, IDisposable
    {
        private readonly ICategoryService _categoryService;

        public CategoryServiceUnitTests(AppDbContext dbContext, ICategoryService categoryService)
            : base(dbContext)
        {
            _categoryService = categoryService;
            SeedData();
        }

        [Fact]
        public async Task GetCategory_ExistingCategoryId_ReturnsCategory()
        {
            int categoryId = 1;

            var category = await _categoryService.GetCategory(c => c.Id == categoryId);

            Assert.NotNull(category);
        }

        [Fact]
        public async Task GetCategory_NonExistingCategoryId_ReturnsNull() 
        {
            int categoryId = 777;

            var category = await _categoryService.GetCategory(c => c.Id == categoryId);

            Assert.Null(category);
        }

        [Fact]
        public async Task GetCategory_NullFilter_ThrowsArgumentNullException()
        {
            Func<Task<Category>> action = async () => await _categoryService.GetCategory(null);

            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task GetCategories_NullFilter_ReturnsAllCategories()
        {
            int expectedCount = 3;

            var categories = await _categoryService.GetCategories();

            Assert.Equal(expectedCount, categories.Count());
        }

        [Fact]
        public async Task GetCategories_NonNullFilterAndExistingResults_ReturnsCategoriesMatchedWithFilter()
        {
            int parentCategoryIdFilter = 1;
            int expectedCount = 1;

            var categories = await _categoryService.GetCategories(c => c.ParentCategoryId == parentCategoryIdFilter);

            Assert.Equal(expectedCount, categories.Count());
        }

        [Fact]
        public async Task GetCategories_NonNullFilterAndNonExistingResults_ReturnsEmptyCollection()
        {
            int parentCategoryIdFilter = 7;

            var categories = await _categoryService.GetCategories(c => c.ParentCategoryId == parentCategoryIdFilter);

            Assert.Empty(categories);
        }

        [Fact]
        public async Task AddOrEditCategory_NullModel_ThrowsArgumentNullException()
        {
            Func<Task> action = async () => await _categoryService.AddOrEditCategory(null);

            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task AddOrEditCategory_NewMainCategory_AddsNewMainCategory()
        {
            var category = new Category()
            {
                Name = "New category",
                ParentCategoryId = null
            };

            await _categoryService.AddOrEditCategory(category);

            var entity = DbContext.Categories.FirstOrDefault(c => c.Name == category.Name);
            Assert.NotNull(entity);
        }

        [Fact]
        public async Task AddOrEditCategory_EditMainCategory_EditsMainCategory()
        {
            var category = await DbContext.Categories.FirstOrDefaultAsync(c => c.Id == 1);
            category.Name = "new";

            await _categoryService.AddOrEditCategory(category);

            var entity = DbContext.Categories.FirstOrDefault(c => c.Name == category.Name);
            Assert.NotNull(entity);
            Assert.Equal(category.Name, entity.Name);
        }

        [Fact]
        public async Task AddOrEditCategory_NewSubcategoryWithNonExistingMainCategory_ThrowsInvalidOperationException()
        {
            var category = new Category()
            {
                Name = "New category",
                ParentCategoryId = 7
            };

            Func<Task> action = async () => await _categoryService.AddOrEditCategory(category);

            await Assert.ThrowsAsync<InvalidOperationException>(action);
        }

        [Fact]
        public async Task AddOrEditCategory_NewSubcategory_AddsNewSubcategory()
        {
            var category = new Category()
            {
                Name = "New subcategory",
                ParentCategoryId = 1
            };

            await _categoryService.AddOrEditCategory(category);

            var entity = DbContext.Categories.FirstOrDefault(c => c.Name == category.Name);
            Assert.NotNull(entity);
            Assert.Equal(category.ParentCategoryId, entity.ParentCategoryId);
        }

        [Fact]
        public async Task DeleteCategory_ExistingCategoryId_DeletesCategory()
        {
            int categoryId = 1;

            await _categoryService.DeleteCategory(categoryId);

            var entity = await DbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            Assert.Null(entity);
        }

        [Fact]
        public async Task DeleteCategory_NonExistentCategoryId_ThrowsInvalidOperationException()
        {
            int categoryId = 777;

            Func<Task> action = async () => await _categoryService.DeleteCategory(categoryId);

            await Assert.ThrowsAsync<InvalidOperationException>(action);
        }

        [Fact]
        public async Task GetCourseOwnersEmails_ExistingCategoryIdExistingCourses_ReturnsEmails()
        {
            int categoryId = 1;
            int expectedCount = 2;

            var emails = await _categoryService.GetCourseOwnersEmails(categoryId);

            Assert.Equal(expectedCount, emails.Count());
        }

        [Fact]
        public async Task GetCourseOwnersEmails_NonExistingCategoryId_ReturnsEmptyCollection()
        {
            int categoryId = 777;

            var emails = await _categoryService.GetCourseOwnersEmails(categoryId);

            Assert.Empty(emails);
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
        }

        private void SeedData()
        {
            var category1 = new Category()
            {
                Id = 1,
                Name = "Programming",
                ParentCategoryId = null
            };

            var category2 = new Category()
            {
                Id = 2,
                Name = "Music",
                ParentCategoryId = null
            };

            var category3 = new Category()
            {
                Id = 3,
                Name = "Programming languages",
                ParentCategoryId = 1
            };

            DbContext.Categories.Add(category1);
            DbContext.Categories.Add(category2);
            DbContext.Categories.Add(category3);

            var owner1 = new CourseOwner()
            {
                Id = 1,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@gmail.com",
            };

            var owner2 = new CourseOwner()
            {
                Id = 2,
                FirstName = "Frank",
                LastName = "Scott",
                Email = "frank.scott@gmail.com",
            };

            DbContext.Users.Add(owner1);
            DbContext.Users.Add(owner2);

            var course1 = new Course()
            {
                Id = 1,
                CategoryId = 1,
                OwnerId = 1
            };

            var course2 = new Course()
            {
                Id = 2,
                CategoryId = 1,
                OwnerId = 2
            };

            DbContext.Courses.Add(course1);
            DbContext.Courses.Add(course2);

            DbContext.SaveChanges();
        }
    }
}
