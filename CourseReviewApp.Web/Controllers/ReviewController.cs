using AutoMapper;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using CourseReviewApp.Web.Services.Interfaces;
using CourseReviewApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Controllers
{
    public class ReviewController : BaseController
    {
        private readonly IReviewService _reviewService;
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;
        private readonly IEmailSenderService _emailSenderService;

        public ReviewController(ILogger logger, IMapper mapper, IReviewService ratingService, UserManager<AppUser> userManager,
            ICourseService courseService, IUserService userService, IEmailSenderService emailSenderService)
            : base(logger, mapper, userManager)
        {
            _reviewService = ratingService;
            _courseService = courseService;
            _userService = userService;
            _emailSenderService = emailSenderService;
        }

        [HttpGet]
        [Authorize(Roles = "Course_client")]
        public async Task<IActionResult> AddOrEditReview(int courseId, int? reviewId = null)
        {
            Course course = await _courseService.GetCourse(c => c.Id == courseId);
            if (course == null)
                return NotFound();
            if (course.Status != CourseStatus.Active)
                return Forbid();

            AddOrEditReviewVm viewModel = null;
            ViewBag.CourseName = course.Name;
            int userId = int.Parse(UserManager.GetUserId(User));

            if (reviewId.HasValue)
            {
                Review review = await _reviewService.GetReview(r => r.Id == reviewId);
                if (review == null)
                    return NotFound();
                if (review.AuthorId != userId)
                    return Forbid();

                viewModel = Mapper.Map<AddOrEditReviewVm>(review);
                TempData["PreviousReviewContents"] = viewModel.Contents;
                TempData["PreviousReviewRating"] = viewModel.RatingValue.ToString();

                return View(viewModel);
            }

            viewModel = new()
            {
                CourseId = courseId,
                AuthorId = userId,
                CourseName = course.Name
            };

            TempData["OwnerEmail"] = course.Owner.CourseInfoEmailsEnabled ? course.Owner.Email : string.Empty;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Course_client")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditReview(AddOrEditReviewVm viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Id.HasValue && viewModel.Contents == TempData["PreviousReviewContents"].ToString() &&
                    viewModel.RatingValue.ToString() == TempData["PreviousReviewRating"].ToString())
                {
                    TempData["CourseDetailsMsgModal"] = "Review has not been edited.";
                    return RedirectToAction("Details", "Course", new { id = viewModel.CourseId });
                }
                await _reviewService.AddOrEditReview(Mapper.Map<Review>(viewModel));

                if(!viewModel.Id.HasValue)
                {
                    IList<string> observingUsers = (await _courseService.GetObservingUsersEmails(viewModel.CourseId,
                    int.Parse(UserManager.GetUserId(User)))).ToList();
                    if (!string.IsNullOrEmpty(TempData["OwnerEmail"].ToString()))
                    {
                        observingUsers.Add(TempData["OwnerEmail"].ToString());
                        TempData.Remove("OwnerEmail");
                    }
                    if (observingUsers.Any())
                        await _emailSenderService.SendDefaultMessageEmailAsync("New course review",
                                $"There is a new review for the {viewModel.CourseName} course.", bccs: observingUsers);
                }
                TempData["CourseDetailsMsgModal"] = viewModel.Id.HasValue ? "Your review has been edited."
                    : "Your review has been added.";

                return RedirectToAction("Details", "Course", new { id = viewModel.CourseId });
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Course_client, Admin, Moderator")]
        public async Task<IActionResult> DeleteReview(int id, bool isModeratingDeletion = false,
            bool returnToReportManagement = false)
        {
            Review review = await _reviewService.GetReview(r => r.Id == id);
            if (review == null)
                return NotFound();
            if (User.IsInRole("Course_client") && !User.IsInRole("Moderator") &&
               review.AuthorId != int.Parse(UserManager.GetUserId(User)))
                return Forbid();

            DeleteReviewVm viewModel = Mapper.Map<DeleteReviewVm>(review);
            viewModel.ReturnUrl = Request.Headers["Referer"].ToString();
            viewModel.IsModeratingDeletion = isModeratingDeletion;
            viewModel.ReturnToReportManagement = returnToReportManagement;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Course_client, Admin, Moderator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(DeleteReviewVm viewModel)
        {
            if (ModelState.IsValid)
            {
                await _reviewService.DeleteReview(viewModel.Id);
                if(viewModel.IsModeratingDeletion)
                    TempData["ReportManagementMsgModal"] = "Review has been deleted.";
                else
                    TempData["CourseDetailsMsgModal"] = "Your review has been deleted.";
                if (viewModel.IsModeratingDeletion && viewModel.Author.ReviewInfoEmailsEnabled)
                    await _emailSenderService.SendDefaultMessageEmailAsync("Review deletion",
                            $"Your review for the {viewModel.CourseName} course has been deleted by moderation due to violation of the rules.",
                            viewModel.Author.Email);

                if (viewModel.ReturnToReportManagement)
                    return RedirectToAction("ReportManagement", "Report");
                return Redirect(viewModel.ReturnUrl);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Course_owner")]
        public async Task<IActionResult> AddOrEditOwnerComment(int reviewId, int? commentId = null)
        {
            Review review = await _reviewService.GetReview(r => r.Id == reviewId);
            if (review == null)
                return NotFound();

            int userId = int.Parse(UserManager.GetUserId(User));
            if (review.Course.OwnerId != userId)
                return Forbid();

            AddOrEditOwnerCommentVm viewModel = new()
            {
                CourseId = review.CourseId,
                Review = Mapper.Map<ReviewVm>(review),
                AuthorId = userId
            };

            if (commentId.HasValue)
            {
                viewModel = Mapper.Map<AddOrEditOwnerCommentVm>(review.OwnerComment);
                viewModel.CourseId = review.CourseId;
                TempData["PreviousCommentContents"] = viewModel.Contents;

                return View(viewModel);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Course_owner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditOwnerComment(AddOrEditOwnerCommentVm viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Id.HasValue && viewModel.Contents == TempData["PreviousCommentContents"].ToString())
                {
                    TempData["CourseDetailsMsgModal"] = "Comment has not been edited.";
                    return RedirectToAction("Details", "Course", new { id = viewModel.CourseId });
                }

                await _reviewService.AddOrEditOwnerComment(Mapper.Map<OwnerComment>(viewModel));
                TempData["CourseDetailsMsgModal"] = viewModel.Id.HasValue ? "Your comment has been edited."
                    : "Your comment has been added.";

                return RedirectToAction("Details", "Course", new { id = viewModel.CourseId });
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Course_client")]
        public async Task<string> VoteForReviewHelpfullness(int id)
        {
            bool result = await _reviewService.VoteForReviewHelpfullness(int.Parse(UserManager.GetUserId(User)), id);
            return result.ToString();
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews(int courseId, string sortOrder, string filterValue,
            bool loadMore, int numberOfReviews = 0)
        {
            IEnumerable<Review> reviews = await _reviewService.GetReviews(r => r.CourseId == courseId);
            int reviewsCount = reviews.Count();
            if (reviewsCount == 0)
                return StatusCode(204);

            ViewBag.LoadBtnDisabled = "";
            ViewBag.LastReviewMsgVisible = "hidden";
            ViewBag.SortOrder = sortOrder;
            ViewBag.FilterValue = filterValue;
            TempData["CourseId"] = courseId;

            bool filterResultsExists = true;
            if (!string.IsNullOrEmpty(filterValue))
            {
                switch (filterValue)
                {
                    case "Rating_1":
                        reviews = reviews.Where(r => r.RatingValue == RatingValue.Rating_1);
                        break;
                    case "Rating_2":
                        reviews = reviews.Where(r => r.RatingValue == RatingValue.Rating_2);
                        break;
                    case "Rating_3":
                        reviews = reviews.Where(r => r.RatingValue == RatingValue.Rating_3);
                        break;
                    case "Rating_4":
                        reviews = reviews.Where(r => r.RatingValue == RatingValue.Rating_4);
                        break;
                    case "Rating_5":
                        reviews = reviews.Where(r => r.RatingValue == RatingValue.Rating_5);
                        break;
                    case "My_review":
                        reviews = reviews.Where(r => r.AuthorId == int.Parse(UserManager.GetUserId(User)));
                        break;
                }

                if (!reviews.Any())
                {
                    reviews = await _reviewService.GetReviews(r => r.CourseId == courseId);
                    filterResultsExists = false;
                }
                else
                {
                    reviewsCount = reviews.Count();
                    numberOfReviews = reviewsCount > 5 ? numberOfReviews : reviewsCount;
                }
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "Worst rating":
                        reviews = reviews.OrderBy(r => r.RatingValue);
                        break;
                    case "Best rating":
                        reviews = reviews.OrderByDescending(r => r.RatingValue);
                        break;
                    case "Oldest":
                        reviews = reviews.OrderBy(r => r.DateAdded);
                        break;
                    case "Newest":
                        reviews = reviews.OrderByDescending(r => r.DateAdded);
                        break;
                    case "Most helpfull":
                        reviews = reviews.OrderByDescending(r => r.HelpfullReviews.Count);
                        break;
                }
            }

            if (numberOfReviews == 0)
                reviews = reviews.Take(5);
            else if (filterResultsExists)
            {
                if ((!string.IsNullOrEmpty(sortOrder) || !string.IsNullOrEmpty(filterValue)) && !loadMore)
                    reviews = reviews.Take(numberOfReviews >= 5 ? numberOfReviews : 5);
                else
                    reviews = reviews.Take(numberOfReviews + 5);
                if ((reviews.Count() == reviewsCount))
                {
                    ViewBag.LoadBtnDisabled = "disabled";
                    ViewBag.LastReviewMsgVisible = "";
                }
            }
            else
            {
                ViewBag.FilterResultsMsg = "No results for selected filter.";
                ViewBag.ReviewsVisible = "hidden";
                ViewBag.LoadBtnDisabled = "disabled";
            }

            return PartialView("_ReviewsPartial", Mapper.Map<IEnumerable<ReviewVm>>(reviews));
        }

        [HttpGet]
        [Authorize(Roles = "Course_owner, Admin, Moderator")]
        public async Task<IActionResult> DeleteOwnerComment(int id, bool isModeratingDeletion = false)
        {
            OwnerComment ownerComment = await _reviewService.GetOwnerComment(ow => ow.Id == id);
            if (ownerComment == null)
                return NotFound();
            if (User.IsInRole("Course_owner") && !User.IsInRole("Moderator") &&
               ownerComment.AuthorId != int.Parse(UserManager.GetUserId(User)))
                return Forbid();

            DeleteOwnerCommentVm viewModel = Mapper.Map<DeleteOwnerCommentVm>(ownerComment);
            viewModel.ReturnUrl = Request.Headers["Referer"].ToString();
            viewModel.IsModeratingDeletion = isModeratingDeletion;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Course_owner, Admin, Moderator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOwnerComment(DeleteOwnerCommentVm viewModel)
        {
            if (ModelState.IsValid)
            {
                await _reviewService.DeleteOwnerComment(viewModel.Id);
                if(viewModel.IsModeratingDeletion)
                    TempData["ReportManagementMsgModal"] = "Owner's comment has been deleted.";
                else
                    TempData["CourseDetailsMsgModal"] = "Your comment has been deleted.";

                return Redirect(viewModel.ReturnUrl);
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LastAddedReviews()
        {
            IEnumerable<Review> reviews = await _reviewService.GetLastAddedReviews(10);
            if (!reviews.Any())
                return StatusCode(204);

            return View("_LastAddedReviewsPartial", Mapper.Map<IEnumerable<ReviewVm>>(reviews));
        }
    }
}
