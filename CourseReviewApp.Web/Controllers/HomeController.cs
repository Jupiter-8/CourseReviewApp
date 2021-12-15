using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Web.ViewModels;
using CourseReviewApp.Web.Services.Interfaces;
using System.Diagnostics;
using System.IO;
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
        public IActionResult Privacy()
        {
            string filePath = "Privacy\\privacy_file.pdf";
            bool fileExists = _fileService.FileExists(filePath);
            ViewBag.FileExists = fileExists;
            if (fileExists)
                ViewBag.FileName = Path.GetFileName(filePath);

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrEditPrivacyFile()
        {
            bool fileExists = !_fileService.FileExists("Privacy\\privacy_file.pdf");
            return View(new AddOrEditPrivacyFileVm() { IsNew = fileExists });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditPrivacyFile(AddOrEditPrivacyFileVm viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.File != null)
                {
                    await _fileService.SavePrivacyFile(viewModel, "Privacy", "privacy_file.pdf");
                    TempData["PrivacyMsgModal"] = "The privacy file has been updated.";

                    return RedirectToAction("Privacy");
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePrivacyFile()
        {
            return View();
        }

        [HttpPost]
        [ActionName("DeletePrivacyFile")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePrivacyFileConfirm()
        {
            _fileService.DeleteFile("Privacy\\privacy_file.pdf");
            TempData["PrivacyMsgModal"] = "The privacy file has been deleted.";

            return RedirectToAction("Privacy");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            ErrorViewModel viewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message
            };

            return View(viewModel);
        }
    }
}
