using AutoMapper;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using CourseReviewApp.Web.Services.Interfaces;
using CourseReviewApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using X.PagedList;

namespace CourseReviewApp.Web.Controllers
{
    public class CourseController : BaseController
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly IFileService _fileService;
        private readonly IEmailSenderService _emailSenderService;

        public CourseController(ILogger logger, IMapper mapper, ICourseService courseService, ICategoryService categoryService,
            UserManager<AppUser> userManager, IWebHostEnvironment hostingEnv, IFileService fileTool,
            IEmailSenderService emailSenderService, IUserService userService)
            : base(logger, mapper, userManager)
        {
            _courseService = courseService;
            _categoryService = categoryService;
            _hostingEnv = hostingEnv;
            _fileService = fileTool;
            _emailSenderService = emailSenderService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string searchString, bool isMainCategory,
            bool notAssignedToCategory, int? page = 1, int? categoryId = null, int? ownerId = null)
        {
            if ((await _courseService.GetCoursesCount()) == 0)
                return View();

            IEnumerable<Course> courses = null;
            Category category = null;
            ViewBag.Action = "Index";
            ViewBag.Controller = "Course";
            ViewBag.NotAssignedToCategoryCount = await _courseService.GetCoursesCount(c => c.CategoryId == null);

            if(notAssignedToCategory)
            {
                courses = await _courseService.GetCourses(c => c.CategoryId == null && c.Status == CourseStatus.Active);
                ViewBag.NotAssignedToCategory = true;
            }
            else if (ownerId.HasValue)
            {
                courses = await _courseService.GetCourses(c => c.OwnerId == ownerId && c.Status == CourseStatus.Active);
                Course course = courses.FirstOrDefault();
                ViewBag.OwnerName = course != null ? $"{course.Owner.FirstName} {course.Owner.LastName}" : string.Empty;
                ViewBag.OwnerId = ownerId;
            }
            else
            {
                if (!categoryId.HasValue)
                {
                    IEnumerable<Category> categories = await _categoryService.GetCategories();
                    category = categories.FirstOrDefault();
                    isMainCategory = true;
                }
                else
                    category = await _categoryService.GetCategory(c => c.Id == categoryId);

                if (category == null)
                    return View();
                else if (isMainCategory)
                    courses = await _courseService.GetCourses(c => c.Category.ParentCategoryId == category.Id && c.Status == CourseStatus.Active);
                else
                    courses = await _courseService.GetCourses(c => c.CategoryId == category.Id && c.Status == CourseStatus.Active);

                ViewBag.IsMainCategory = isMainCategory;
                ViewBag.CategoryName = category.Name;
                ViewBag.CategoryId = category.Id;
                ViewBag.ParentCategoryId = !isMainCategory ? category.ParentCategoryId : 0;
                ViewBag.ParentCategoryName = !isMainCategory ? category.ParentCategory.Name : string.Empty;
                TempData["ExpandedCategoryId"] = isMainCategory ? category.Id : category.ParentCategoryId;
            }

            ViewBag.SearchString = searchString;
            ViewBag.SortOrder = sortOrder;
            if (!string.IsNullOrEmpty(searchString))
                courses = courses.Where(c => c.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));

            switch (sortOrder)
            {
                case "Oldest":
                    courses = courses.OrderBy(c => c.DateAdded);
                    break;
                case "Newest":
                    courses = courses.OrderByDescending(c => c.DateAdded);
                    break;
                case "Worst rating":
                    courses = courses.OrderBy(c => c.AvgRating);
                    break;
                case "Best rating":
                    courses = courses.OrderByDescending(c => c.AvgRating);
                    break;
                case "Name ascending":
                    courses = courses.OrderBy(c => c.Name);
                    break;
                case "Name descending":
                    courses = courses.OrderByDescending(c => c.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IEnumerable<CourseLessDetailsVm> courseVms = Mapper.Map<IEnumerable<CourseLessDetailsVm>>(courses);
            ViewBag.ResultsExist = courseVms.Any() ? "hidden" : string.Empty;

            return View(courseVms.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin")]
        public async Task<IActionResult> CourseManagement()
        {
            IEnumerable<Course> courses = await _courseService.GetCourses();
            ViewBag.ModeratingActions = true;

            return View(Mapper.Map<IEnumerable<BaseCourseVm>>(courses));
        }

        [HttpGet]
        [Authorize(Roles = "Course_owner")]
        public async Task<IActionResult> OwnerCoursesManagement()
        {
            IEnumerable<Course> courses = await _courseService.GetCourses(
                c => c.OwnerId == int.Parse(UserManager.GetUserId(User)));
            ViewBag.ModeratingActions = false;

            return View("CourseManagement", Mapper.Map<IEnumerable<BaseCourseVm>>(courses));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Course course = await _courseService.GetCourse(c => c.Id == id);
            if (course == null)
                return NotFound();

            string userId = UserManager.GetUserId(User);
            if (course.Status == CourseStatus.Active || (!string.IsNullOrEmpty(userId) && (User.IsInRole("Admin") 
                || User.IsInRole("Moderator") || course.OwnerId == int.Parse(userId))))
            {
                if (User.IsInRole("Course_client"))
                    ViewBag.IsObserved = course.ObservingUsers.Any(o => o.UserId == int.Parse(userId));
                ViewBag.Percentage = CalculateRatingPercentage(course.Reviews);
                if (course.CategoryId.HasValue)
                {
                    ViewBag.CategoryName = course.Category.Name;
                    ViewBag.ParentCategoryName = course.Category.ParentCategory.Name;
                    ViewBag.CategoryId = course.Category.Id;
                    ViewBag.ParentCategoryId = course.Category.ParentCategoryId;
                }

                return View(Mapper.Map<CourseFullDetailsVm>(course));
            }

            return Forbid();
        }

        private static List<(int, double)> CalculateRatingPercentage(IList<Review> reviews)
        {
            List<(int, double)> percentages = new(5);
            int reviewsCount = reviews.Count;
            double percentage = 0.0;
            for (int i = 1; i < 6; i++)
            {
                percentage = Math.Round((double)reviews.Count(r => (int)r.RatingValue == i) / reviewsCount * 100);
                (int, double) item = (reviews.Count(r => (int)r.RatingValue == i), double.IsNaN(percentage) ? 0.0 : percentage);
                percentages.Add(item);
            }

            return percentages;
        }

        [HttpGet]
        [Authorize(Roles = "Course_owner, Admin, Moderator")]
        public async Task<IActionResult> ManagementDetails(int id)
        {
            Course course = await _courseService.GetCourse(c => c.Id == id);
            if (course == null)
                return NotFound();
            if (User.IsInRole("Course_owner"))
            {
                if (course.OwnerId != int.Parse(UserManager.GetUserId(User)))
                    return Forbid();
            }

            return View(Mapper.Map<CourseFullDetailsVm>(course));
        }

        [HttpGet]
        [Authorize(Roles = "Course_owner")]
        public async Task<IActionResult> AddOrEditCourse(int? id = null)
        {
            AddOrEditCourseVm viewModel = null;
            IEnumerable<Category> categories = await _categoryService.GetCategories(c => !c.ParentCategoryId.HasValue);
            List<SelectListItem> categoriesSelectList = categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            ViewBag.Categories = categoriesSelectList;
            int userId = int.Parse(UserManager.GetUserId(User));

            if (id.HasValue)
            {
                Course course = await _courseService.GetCourse(c => c.Id == id);
                if (course == null)
                    return NotFound();
                if (course.OwnerId != userId)
                    return Forbid();

                viewModel = Mapper.Map<AddOrEditCourseVm>(course);
                viewModel.ParentCategoryId = course.Category?.ParentCategoryId;
                while (viewModel.LearningSkills.Count != 10)
                    viewModel.LearningSkills.Add(new LearningSkillVm());

                viewModel.DateEdited = null;
                TempData["PreviousCourseDate"] = JsonSerializer.Serialize(viewModel.DateAdded);
                TempData["PreviousCourseState"] = JsonSerializer.Serialize(viewModel);

                return View(viewModel);
            }

            viewModel = new AddOrEditCourseVm()
            {
                OwnerId = userId,
                LearningSkills = new List<LearningSkillVm>(10),
            };

            for (int i = 0; i < 10; i++)
                viewModel.LearningSkills.Add(new LearningSkillVm());

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Course_owner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditCourse(AddOrEditCourseVm viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Id.HasValue && JsonSerializer.Serialize(viewModel) == TempData["PreviousCourseState"].ToString())
                {
                    viewModel.DateAdded = JsonSerializer.Deserialize<DateTimeOffset>(TempData["PreviousCourseDate"].ToString());
                    TempData["CourseManagementMsgModal"] = "Course has not been edited.";

                    return RedirectToAction("OwnerCoursesManagement");
                }

                string destFolder = "Images\\Courses";
                if (viewModel.Image != null)
                    viewModel.ImagePath = await _fileService.SaveCourseImage(viewModel, destFolder);
                else if (viewModel.Image == null && !string.IsNullOrEmpty(viewModel.ImagePath) && viewModel.ImgToDelete
                    && viewModel.ImagePath != "default_course_image.jpg")
                {
                    _fileService.DeleteFile(Path.Combine(destFolder, viewModel.ImagePath));
                    viewModel.ImagePath = "default_course_image.jpg";
                }
                else if (!viewModel.Id.HasValue && viewModel.Image == null)
                    viewModel.ImagePath = "default_course_image.jpg";

                await _courseService.AddOrEditCourse(Mapper.Map<Course>(viewModel));
                string word = viewModel.Id.HasValue ? "edited" : "added";
                TempData["CourseManagementMsgModal"] = $"'{viewModel.Name}' course has been {word}." +
                    $" It will receive an Active status after positive verification by moderation.";

                return RedirectToAction("OwnerCoursesManagement");
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Course_owner, Admin")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            Course course = await _courseService.GetCourse(c => c.Id == id);
            if (course == null)
                return NotFound();
            if (User.IsInRole("Course_owner"))
            {
                if (course.OwnerId != int.Parse(UserManager.GetUserId(User)))
                    return Forbid();
            }

            return View(Mapper.Map<DeleteCourseVm>(course));
        }

        [HttpPost]
        [Authorize(Roles = "Course_owner, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(DeleteCourseVm viewModel)
        {
            if (viewModel.ImagePath != "default_course_image.jpg")
                _fileService.DeleteFile(Path.Combine("Images\\Courses", viewModel.ImagePath));
            await _courseService.DeleteCourse(viewModel.Id);
            if (User.IsInRole("Admin") || viewModel.OwnerHasCourseInfoEmailsEnabled)
            {
                await _emailSenderService.SendDefaultMessageEmailAsync("Course deletion",
                        $"Your course: {viewModel.Name} has been deleted by Admin.", viewModel.OwnerEmail);
            }
            TempData["CourseManagementMsgModal"] = $"'{viewModel.Name}' course has been deleted";

            return RedirectToAction(User.IsInRole("Course_owner") 
                ? "OwnerCoursesManagement" : "CourseManagement");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> ChangeCourseStatus(int id)
        {
            Course course = await _courseService.GetCourse(c => c.Id == id);
            if (course == null)
                return NotFound();
            var statusValues = from CourseStatus status in Enum.GetValues(typeof(CourseStatus))
                             select new { id = (int)status, name = status.ToString() };

            ViewBag.StatusValues = new SelectList(statusValues, "id", "name");
            TempData["PreviousCourseStatus"] = course.Status.ToString();

            return View(Mapper.Map<ChangeCourseStatusVm>(course));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeCourseStatus(ChangeCourseStatusVm viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Status.ToString() == TempData["PreviousCourseStatus"].ToString())
                {
                    TempData["CourseManagementMsgModal"] = $"The status of the '{viewModel.Name}' course has not been changed.";
                    return RedirectToAction("CourseManagement");
                }
                await _courseService.ChangeCourseStatus(viewModel.Id, viewModel.Status);

                if (viewModel.OwnerHasCourseInfoEmailsEnabled)
                {
                    await _emailSenderService.SendDefaultMessageEmailAsync("Course status",
                        $"The status of your course: {viewModel.Name} has been changed to {viewModel.Status} by moderation.",
                        viewModel.OwnerEmail);
                }
                TempData["CourseManagementMsgModal"] = $"The status of the '{viewModel.Name}' course has been changed to {viewModel.Status}";

                return RedirectToAction("CourseManagement");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LastAddedCourses()
        {
            IEnumerable<Course> courses = await _courseService.GetLastAddedCourses(10);
            if (!courses.Any())
                return StatusCode(204);
            ViewBag.CarouselId = "last-added-courses-carousel";

            return PartialView("_CarouselCoursesPartial", Mapper.Map<IEnumerable<CourseLessDetailsVm>>(courses));
        }

        [HttpGet]
        public async Task<IActionResult> BestRatedCourses()
        {
            IEnumerable<Course> courses = await _courseService.GetBestRatedCourses(10);
            if (!courses.Any())
                return StatusCode(204);
            ViewBag.CarouselId = "best-rated-courses-carousel";

            return PartialView("_CarouselCoursesPartial", Mapper.Map<IEnumerable<CourseLessDetailsVm>>(courses));
        }

        [HttpPost]
        [Authorize(Roles = "Course_client")]
        public async Task AddCourseToObservedList(int id) 
        {
            await _courseService.AddCourseToObservedList(int.Parse(UserManager.GetUserId(User)), id);
        }

        [HttpPost]
        [Authorize(Roles = "Course_client")]
        public async Task RemoveCourseFromObservedList(int id)
        {
            await _courseService.RemoveCourseFromObservedList(int.Parse(UserManager.GetUserId(User)), id); 
        }

        [HttpGet]
        [Authorize(Roles = "Course_client")]
        public async Task<IActionResult> ObservedCoursesList()
        {
            IEnumerable<ObservedCourse> observedCourses = await _courseService.GetObservedCourses(int.Parse(UserManager.GetUserId(User)));
            return View(Mapper.Map<IEnumerable<ObservedCourseVm>>(observedCourses));
        }
    }
}
