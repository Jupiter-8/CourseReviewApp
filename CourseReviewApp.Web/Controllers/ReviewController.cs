﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using CourseReviewApp.Web.ViewModels;
using CourseReviewApp.Web.Services.Interfaces;
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
                return RedirectToAction("Error", "Home", new { message = "Course not found." });

            AddOrEditReviewVm viewModel = null;
            ViewBag.CourseName = course.Name;
            int userId = int.Parse(UserManager.GetUserId(User));

            if (reviewId.HasValue)
            {
                Review review = await _reviewService.GetReview(r => r.Id == reviewId);
                if (review == null)
                    return RedirectToAction("Error", "Home", new { message = "Review not found." });
                if (review.AuthorId != userId)
                    return RedirectToAction("Error", "Home", new { message = "You dont't have access to this resource." });

                viewModel = Mapper.Map<AddOrEditReviewVm>(review);
                TempData["PreviousReviewContents"] = viewModel.Contents;
                TempData["PreviousReviewRating"] = viewModel.RatingValue.ToString();

                return View(viewModel);
            }

            viewModel = new AddOrEditReviewVm()
            {
                CourseId = courseId,
                AuthorId = userId
            };

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
                    TempData["CourseDetailsMsgModal"] = "No changes made. Review has not been edited.";
                    TempData.Remove("PreviousReviewContents");
                    TempData.Remove("PreviousReviewRating");
                    return RedirectToAction("Details", "Course", new { id = viewModel.CourseId });
                }

                await _reviewService.AddOrEditReview(Mapper.Map<Review>(viewModel));
                TempData.Remove("PreviousReviewContents");
                TempData.Remove("PreviousReviewRating");
                TempData["CourseDetailsMsgModal"] = viewModel.Id.HasValue ? "Your review has been edited."
                    : "Your review has been added.";

                return RedirectToAction("Details", "Course", new { id = viewModel.CourseId });
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Course_client, Admin, Moderator")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            Review review = await _reviewService.GetReview(r => r.Id == id);
            if (review == null)
                return RedirectToAction("Error", "Home", new { message = "Review not found." });
            if (User.IsInRole("Course_client"))
            {
                if (review.AuthorId != int.Parse(UserManager.GetUserId(User)))
                    return RedirectToAction("Error", "Home", new { message = "You dont't have access to this resource." });
            }

            return View(Mapper.Map<ReviewVm>(review));
        }

        [HttpPost]
        [Authorize(Roles = "Course_client, Admin, Moderator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(ReviewVm viewModel)
        {
            if (ModelState.IsValid)
            {
                await _reviewService.DeleteReview(viewModel.Id);
                if ((User.IsInRole("Admin") || User.IsInRole("Moderator")) && viewModel.Author.ReviewInfoEmailsEnabled)
                {
                    await _emailSenderService.SendDefaultMessageEmailAsync(viewModel.Author.Email, "Review deletion",
                            $"Your review for course: {viewModel.CourseName} has been deleted by moderation due to violation of the rules.");
                    TempData["ReportManagementMsgModal"] = "Review has been deleted.";

                    return RedirectToAction("ReportManagement", "Report");
                }
                TempData["CourseDetailsMsgModal"] = "Your review has been deleted.";

                return RedirectToAction("Details", "Course", new { id = viewModel.CourseId });
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Course_owner")]
        public async Task<IActionResult> AddOrEditOwnerComment(int reviewId, int? commentId = null)
        {
            Review review = await _reviewService.GetReview(r => r.Id == reviewId);
            if (review == null)
                return RedirectToAction("Error", "Home", new { message = "Review not found." });

            int userId = int.Parse(UserManager.GetUserId(User));
            if(review.Course.OwnerId != userId)
                return RedirectToAction("Error", "Home", new { message = "You dont't have access to this resource." });

            AddOrEditOwnerCommentVm viewModel = new AddOrEditOwnerCommentVm()
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
                    TempData["CourseDetailsMsgModal"] = "No changes made. Comment has not been edited.";
                    TempData.Remove("PreviousCommentContents");
                    return RedirectToAction("Details", "Course", new { id = viewModel.CourseId });
                }

                await _reviewService.AddOrEditOwnerComment(Mapper.Map<OwnerComment>(viewModel));
                TempData.Remove("PreviousCommentContents");
                TempData["CourseDetailsMsgModal"] = viewModel.Id.HasValue ? "Your comment has been edited."
                    : "Your comment has been added.";

                return RedirectToAction("Details", "Course", new { id = viewModel.CourseId });
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReviewDetails(int id)
        {
            Review review = await _reviewService.GetReview(r => r.Id == id);
            if (review == null)
                return RedirectToAction("Error", "Home", new { message = "Review not found." });

            return View(Mapper.Map<ReviewVm>(review));
        }

        [HttpPost]
        [Authorize(Roles = "Course_client")]
        public async Task<string> VoteForReviewHelpfullness(int id)
        {
            string userId = UserManager.GetUserId(User);
            bool result = await _reviewService.VoteForReviewHelpfullness(int.Parse(userId), id);

            return result.ToString();
        }

        [HttpGet]
        public IActionResult GetReviews(int courseId, string sortOrder, int filterValue, bool loadMore, int numberOfReviews = 0)
        {
            IEnumerable<Review> reviews = _reviewService.GetReviews(r => r.CourseId == courseId);
            int reviewsCount = reviews.Count();
            if (reviewsCount == 0)
                return StatusCode(204);

            ViewBag.LoadBtnDisabled = "";
            ViewBag.LastReviewMsgVisible = "hidden";
            ViewBag.SortOrder = sortOrder;
            ViewBag.FilterValue = filterValue;
            TempData["CourseId"] = courseId;

            bool filterResultsExists = true;
            if (filterValue != 0)
            {
                switch (filterValue)
                {
                    case 1:
                        reviews = reviews.Where(r => r.RatingValue == RatingValue.Rating_1);
                        break;
                    case 2:
                        reviews = reviews.Where(r => r.RatingValue == RatingValue.Rating_2);
                        break;
                    case 3:
                        reviews = reviews.Where(r => r.RatingValue == RatingValue.Rating_3);
                        break;
                    case 4:
                        reviews = reviews.Where(r => r.RatingValue == RatingValue.Rating_4);
                        break;
                    case 5:
                        reviews = reviews.Where(r => r.RatingValue == RatingValue.Rating_5);
                        break;
                }

                if (!reviews.Any())
                {
                    reviews = _reviewService.GetReviews(r => r.CourseId == courseId);
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
                if ((!string.IsNullOrEmpty(sortOrder) || filterValue != 0) && !loadMore)
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
        public async Task<IActionResult> DeleteOwnerComment(int id)
        {
            OwnerComment ownerComment = await _reviewService.GetOwnerComment(ow => ow.Id == id);
            if (ownerComment == null)
                return RedirectToAction("Error", "Home", new { message = "Owner's comment not found." });
            if (User.IsInRole("Course_owner") && ownerComment.AuthorId != int.Parse(UserManager.GetUserId(User)))
                return RedirectToAction("Error", "Home", new { message = "You dont't have access to this resource." });

            return View(Mapper.Map<OwnerCommentVm>(ownerComment));
        }

        [HttpPost]
        [Authorize(Roles = "Course_owner, Admin, Moderator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOwnerComment(OwnerCommentVm viewModel)
        {
            if (ModelState.IsValid)
            {
                await _reviewService.DeleteOwnerComment(viewModel.Id);
                if (User.IsInRole("Admin") || User.IsInRole("Moderator"))
                {
                    TempData["ReportManagementMsgModal"] = "Owner's comment has been deleted.";
                    return RedirectToAction("ReportManagement", "Report");
                }
                else
                {
                    TempData["CourseDetailsMsgModal"] = "Your comment has been deleted.";
                    return RedirectToAction("Details", "Course", new { id = TempData["CourseId"] });
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult LastAddedReviews()
        {
            IEnumerable<Review> reviews = _reviewService.GetReviews().OrderByDescending(r => r.DateAdded).Take(5);
            if (!reviews.Any())
                return StatusCode(204);

            return View("_LastAddedReviewsPartial", Mapper.Map<IEnumerable<ReviewVm>>(reviews));
        }
    }
}
