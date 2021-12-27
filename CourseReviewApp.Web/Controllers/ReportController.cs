using AutoMapper;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using CourseReviewApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly IReviewService _reviewService;

        public ReportController(ILogger logger, IMapper mapper, UserManager<AppUser> userManager, IReportService reportService, IReviewService reviewService)
            : base(logger, mapper, userManager)
        {
            _reportService = reportService;
            _reviewService = reviewService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> ReportManagement()
        {
            IEnumerable<ReviewReportVm> reviewReportVms = Mapper.Map<IEnumerable<ReviewReportVm>>(await _reportService.GetReviewReports());
            return View(reviewReportVms);
        }

        [HttpGet]
        [Authorize(Roles = "Course_client, Course_owner")]
        public async Task<IActionResult> ReportReview(int id)
        {
            Review review = await _reviewService.GetReview(r => r.Id == id);
            if (review == null)
                return NotFound();

            ReportReviewVm viewModel = new()
            {
                ReviewId = id,
                ReportingUserId = int.Parse(UserManager.GetUserId(User)),
                Review = Mapper.Map<ReviewVm>(review)
            };

            var enumValues = from ReportReason r in Enum.GetValues(typeof(ReportReason))
                             select new
                             {
                                 id = (int)r,
                                 name = r.ToString().Replace('_', ' ')
                             };
            ViewBag.ReportReasonValues = new SelectList(enumValues, "id", "name");

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Course_client, Course_owner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportReview(ReportReviewVm viewModel)
        {
            if (ModelState.IsValid)
            {
                await _reportService.AddReviewReport(Mapper.Map<ReviewReport>(viewModel));
                TempData["CourseDetailsMsgModal"] = "Review has been reported.";

                return RedirectToAction("Details", "Course", new { id = TempData["CourseId"].ToString() });
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> DeleteReviewReport(int id)
        {
            ReviewReport reviewReport = await _reportService.GetReviewReport(rr => rr.Id == id);
            if (reviewReport == null)
                return NotFound();

            return View(Mapper.Map<ReviewReportVm>(reviewReport));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReviewReport(ReviewReportVm viewModel)
        {
            if (ModelState.IsValid)
            {
                await _reportService.DeleteReviewReport(viewModel.Id);
                TempData["ReportManagementMsgModal"] = "Review report has been deleted.";

                return RedirectToAction("ReportManagement");
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> ReportDetails(int id)
        {
            ReviewReport reviewReport = await _reportService.GetReviewReport(rr => rr.Id == id);
            if (reviewReport == null)
                return NotFound();

            return View(Mapper.Map<ReviewReportVm>(reviewReport));
        }
    }
}
