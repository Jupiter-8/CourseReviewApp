using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CourseReviewApp.Tests.UnitTests
{
    public class CategoryServiceUnitTests : BaseUnitTests
    {
        private readonly ICategoryService _categoryService;

        public CategoryServiceUnitTests(AppDbContext dbContext, ICategoryService categoryService)
            : base(dbContext)
        {
            _categoryService = categoryService;
        }

        [Fact]
        public async Task GetCategory_ExistingId_ReturnsCategory()
        {
            const int CategoryId = 1;

            var category = await _categoryService.GetCategory(c => c.Id == CategoryId);

            Assert.NotNull(category);
        }

        [Fact]
        public async Task GetCategory_NonExistentId_ReturnsNull()
        {
            const int CategoryId = 777;

            var category = await _categoryService.GetCategory(c => c.Id == CategoryId);

            Assert.Null(category);
        }

        [Fact]
        public async Task GetCategory_NullFilter_ThrowsArgumentNullException()
        {
            Func<Task<Category>> action = async () => await _categoryService.GetCategory(null);

            await Assert.ThrowsAsync<ArgumentNullException>(action);
        }
    }
}
