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

        [Route("Error/{code:int}")]
        public IActionResult Error(int code)
        {
            if (code == 403)
                return View("Error403");
            else if (code == 404)
                return View("Error404");

            IExceptionHandlerPathFeature exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            Logger.LogError(exceptionDetails.Error.ToString());

            return View("Error");
        }
    }
}
