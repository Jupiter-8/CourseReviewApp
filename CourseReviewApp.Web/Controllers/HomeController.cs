using AutoMapper;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Web.Services.Interfaces;
using CourseReviewApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IFileService _fileService;

        public HomeController(ILogger logger, IMapper mapper, UserManager<AppUser> userManager, IFileService fileService)
            : base(logger, mapper, userManager)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TermsAndConditions()
        {
            ViewBag.FileExists = _fileService.FileExists("Documents\\terms_and_conditions_file.pdf");
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrEditTermsAndConditionsFile()
        {
            bool fileExists = !_fileService.FileExists("Documents\\terms_and_conditions_file.pdf");
            return View(new AddOrEditTermsAndConditionsFileVm() { IsNew = fileExists });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditTermsAndConditionsFile(AddOrEditTermsAndConditionsFileVm viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.File != null)
                {
                    await _fileService.SaveTermsAndConditionsFile(viewModel, "Documents", "terms_and_conditions_file.pdf");
                    TempData["TermsAndConditionsMsgModal"] = "Terms and conditions file has been updated.";

                    return RedirectToAction("TermsAndConditions");
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteTermsAndConditionsFile()
        {
            return View();
        }

        [HttpPost]
        [ActionName("DeleteTermsAndConditionsFile")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTermsAndConditionsFileConfirm()
        {
            _fileService.DeleteFile("Documents\\terms_and_conditions_file.pdf");
            TempData["TermsAndConditionsMsgModal"] = "Terms and conditions file has been deleted.";

            return RedirectToAction("TermsAndConditions");
        }
    }
}
