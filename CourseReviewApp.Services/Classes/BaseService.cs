using AutoMapper;
using Microsoft.Extensions.Logging;
using CourseReviewApp.DAL.EntityFramework;

namespace CourseReviewApp.Services.Classes
{
    public abstract class BaseService
    {
        protected readonly AppDbContext DbContext;
        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;

        protected BaseService(AppDbContext dbContext, ILogger logger, IMapper mapper)
        {
            DbContext = dbContext;
            Logger = logger;
            Mapper = mapper;
        }
    }
}
