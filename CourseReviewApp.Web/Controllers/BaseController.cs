using AutoMapper;
using CourseReviewApp.Model.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CourseReviewApp.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;
        protected readonly UserManager<AppUser> UserManager;

        public BaseController(ILogger logger, IMapper mapper, UserManager<AppUser> userManager)
        {
            Logger = logger;
            Mapper = mapper;
            UserManager = userManager;
        }
    }
}
