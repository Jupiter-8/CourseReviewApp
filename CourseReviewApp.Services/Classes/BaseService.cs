using CourseReviewApp.DAL.EntityFramework;

namespace CourseReviewApp.Services.Classes
{
    public abstract class BaseService
    {
        protected readonly AppDbContext DbContext;

        protected BaseService(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
