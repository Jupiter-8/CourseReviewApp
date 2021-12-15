using Microsoft.Extensions.Logging;

namespace CourseReviewApp.Web.Services.Classes
{
    public abstract class BaseService
    {
        protected readonly ILogger Logger;

        protected BaseService(ILogger logger)
        {
            Logger = logger;
        }
    }
}
