using CourseReviewApp.DAL.EntityFramework;

namespace CourseReviewApp.Tests.UnitTests
{
    public abstract class BaseUnitTests
    {
        protected readonly AppDbContext DbContext;

        public BaseUnitTests(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
