using AutoMapper;
using CourseReviewApp.Model.DataModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CourseReviewApp.Web.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController(ILogger logger, IMapper mapper, UserManager<AppUser> userManager) 
            : base(logger, mapper, userManager)
        {
        }

        [Route("Error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            Logger.LogError(exceptionDetails.Error.ToString());

            return View("Error");
        }
    }
}
