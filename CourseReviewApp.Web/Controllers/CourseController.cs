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
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

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
            UserManager<AppUser> userManager, IWebHostEnvironment hostingEnv, IFileService fileTool, IEmailSenderService emailSenderService, IUserService userService)
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
        public async Task<IActionResult> Index(string sortOrder, string searchString, bool isMainCategory, int? page = 1,
            int? categoryId = null, int? ownerId = null)
        {
            if ((await _courseService.GetCoursesCount()) == 0)
                return View();

            IEnumerable<Course> courses = null;
            Category category = null;
            ViewBag.Action = "Index";
            ViewBag.Controller = "Course";

            if (ownerId.HasValue)
            {
                courses = _courseService.GetCourses(c => c.OwnerId == ownerId && c.Status == CourseStatus.Active);
                Course course = courses.FirstOrDefault();
                ViewBag.OwnerName = course != null ? $"{course.Owner.FirstName} {course.Owner.LastName}" : string.Empty;
                ViewBag.OwnerId = ownerId;
            }
            else
            {
                if (!categoryId.HasValue)
                {
                    IEnumerable<Category> categories = _categoryService.GetCategories();
                    category = categories.FirstOrDefault();
                    isMainCategory = true;
                }
                else
                    category = await _categoryService.GetCategory(c => c.Id == categoryId);

                if (category == null)
                    return View();
                else if (isMainCategory)
                    courses = _courseService.GetCourses(c => c.Category.ParentCategoryId == category.Id && c.Status == CourseStatus.Active);
                else
                    courses = _courseService.GetCourses(c => c.CategoryId == category.Id && c.Status == CourseStatus.Active);

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

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            IEnumerable<CourseVm> courseVms = Mapper.Map<IEnumerable<CourseVm>>(courses);
            ViewBag.ResultsExist = courseVms.Any() ? "hidden" : string.Empty;

            return View(courseVms.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin")]
        public IActionResult CourseManagement()
        {
            IEnumerable<Course> courses = _courseService.GetCourses();
            ViewBag.ModeratingActions = true;

            return View(Mapper.Map<IEnumerable<CourseVm>>(courses));
        }

        [HttpGet]
        [Authorize(Roles = "Course_owner")]
        public IActionResult OwnerCoursesManagement()
        {
            string userId = UserManager.GetUserId(User);
            IEnumerable<Course> courses = _courseService.GetCourses(c => c.OwnerId == int.Parse(userId));
            ViewBag.ModeratingActions = false;

            return View("CourseManagement", Mapper.Map<IEnumerable<CourseVm>>(courses));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Course course = await _courseService.GetCourse(c => c.Id == id);
            if (course == null)
                throw new InvalidOperationException($"Course with id: {id} not found.");

            string userId = UserManager.GetUserId(User);
            if (course.Status == CourseStatus.Active || (!string.IsNullOrEmpty(userId) && (User.IsInRole("Admin") || User.IsInRole("Moderator")
                || course.OwnerId == int.Parse(userId))))
            {
                int reviewCount = course.Reviews.Count > 0 ? course.Reviews.Count : 1;
                double starPercent = 0.00;

                starPercent = Math.Round(((double)course.Reviews.Where(r => r.RatingValue == RatingValue.Rating_5).Count() / reviewCount) * 100);
                ViewBag.fiveStarPercent = double.IsNaN(starPercent) ? 0 : starPercent;
                starPercent = Math.Round(((double)course.Reviews.Where(r => r.RatingValue == RatingValue.Rating_4).Count() / reviewCount) * 100);
                ViewBag.fourStarPercent = double.IsNaN(starPercent) ? 0 : starPercent;
                starPercent = Math.Round(((double)course.Reviews.Where(r => r.RatingValue == RatingValue.Rating_3).Count() / reviewCount) * 100);
                ViewBag.threeStarPercent = double.IsNaN(starPercent) ? 0 : starPercent;
                starPercent = Math.Round(((double)course.Reviews.Where(r => r.RatingValue == RatingValue.Rating_2).Count() / reviewCount) * 100);
                ViewBag.twoStarPercent = double.IsNaN(starPercent) ? 0 : starPercent;
                starPercent = Math.Round(((double)course.Reviews.Where(r => r.RatingValue == RatingValue.Rating_1).Count() / reviewCount) * 100);
                ViewBag.oneStarPercent = double.IsNaN(starPercent) ? 0 : starPercent;

                if (course.CategoryId.HasValue)
                {
                    ViewBag.CategoryName = course.Category.Name;
                    ViewBag.ParentCategoryName = course.Category.ParentCategory.Name;
                    ViewBag.CategoryId = course.Category.Id;
                    ViewBag.ParentCategoryId = course.Category.ParentCategoryId;
                }

                return View(Mapper.Map<CourseVm>(course));
            }

            throw new UnauthorizedAccessException("No access to a resource.");
        }

        [HttpGet]
        [Authorize(Roles = "Course_owner, Admin, Moderator")]
        public async Task<IActionResult> ManagementDetails(int id)
        {
            Course course = await _courseService.GetCourse(c => c.Id == id);
            if (course == null)
                throw new InvalidOperationException($"Course with id: {id} not found.");
            if (User.IsInRole("Course_owner"))
            {
                if (course.OwnerId != int.Parse(UserManager.GetUserId(User)))
                    throw new UnauthorizedAccessException("No access to a resource.");
            }

            return View(Mapper.Map<CourseVm>(course));
        }

        [HttpGet]
        [Authorize(Roles = "Course_owner")]
        public async Task<IActionResult> AddOrEditCourse(int? id = null)
        {
            AddOrEditCourseVm viewModel = null;
            IEnumerable<Category> categories = _categoryService.GetCategories(c => !c.ParentCategoryId.HasValue);
            List<SelectListItem> categoriesSelectList = categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            ViewBag.Categories = categoriesSelectList;
            int userId = int.Parse(UserManager.GetUserId(User));

            if (id.HasValue)
            {
                Course course = await _courseService.GetCourse(c => c.Id == id);
                if (course == null)
                    throw new InvalidOperationException($"Course with id: {id} not found.");
                if (course.OwnerId != userId)
                    throw new UnauthorizedAccessException("No access to a resource.");

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
                    viewModel.DateAdded = JsonSerializer.Deserialize<DateTime>(TempData["PreviousCourseDate"].ToString());
                    TempData["CourseManagementMsgModal"] = "Course has not been edited.";

                    return RedirectToAction("OwnerCoursesManagement");
                }

                string destFolder = "Images\\Courses";
                if (viewModel.Image != null)
                {
                    string imgPath = await _fileService.SaveCourseImage(viewModel, destFolder);
                    viewModel.ImagePath = imgPath;
                }
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
                TempData["CourseManagementMsgModal"] = $"The {viewModel.Name} course has been {word}. It will receive an Active status after positive verification by moderation.";

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
                throw new InvalidOperationException($"Course with id: {id} not found.");
            if (User.IsInRole("Course_owner"))
            {
                int ownerId = int.Parse(UserManager.GetUserId(User));
                if (course.OwnerId != ownerId)
                    throw new UnauthorizedAccessException("No access to a resource.");
            }

            return View(Mapper.Map<DeleteCourseVm>(course));
        }

        [HttpPost]
        [Authorize(Roles = "Course_owner, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(DeleteCourseVm viewModel)
        {
            if (viewModel.ImagePath != "default_course_image.jpg")
            {
                string destFolder = Path.Combine(_hostingEnv.WebRootPath, "Images\\Courses");
                _fileService.DeleteFile(Path.Combine(destFolder, viewModel.ImagePath));
            }
            await _courseService.DeleteCourse(viewModel.Id);

            if (User.IsInRole("Admin") || viewModel.OwnerHasCourseInfoEmailsEnabled)
            {
                await _emailSenderService.SendDefaultMessageEmailAsync("Course deletion",
                        $"Your course: {viewModel.Name} has been deleted by Admin.", viewModel.OwnerEmail);
            }

            TempData["CourseManagementMsgModal"] = $"The {viewModel.Name} course has been deleted";
            if (User.IsInRole("Course_owner"))
                return RedirectToAction("OwnerCoursesManagement");
            else
                return RedirectToAction("CourseManagement");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> ChangeCourseStatus(int id)
        {
            Course course = await _courseService.GetCourse(c => c.Id == id);
            if (course == null)
                throw new InvalidOperationException($"Course with id: {id} not found.");

            var enumValues = from CourseStatus s in Enum.GetValues(typeof(CourseStatus))
                             select new
                             {
                                 id = (int)s,
                                 name = s.ToString()
                             };

            ViewBag.StatusValues = new SelectList(enumValues, "id", "name");
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
                    TempData["CourseManagementMsgModal"] = $"The status of the course {viewModel.Name} has not been changed.";
                    return RedirectToAction("CourseManagement");
                }
                await _courseService.ChangeCourseStatus(viewModel.Id, viewModel.Status);

                if (viewModel.OwnerHasCourseInfoEmailsEnabled)
                {
                    await _emailSenderService.SendDefaultMessageEmailAsync("Course status",
                        $"The status of your course: {viewModel.Name} has been changed to {viewModel.Status} by moderation.", viewModel.OwnerEmail);
                }
                TempData["CourseManagementMsgModal"] = $"The status of the {viewModel.Name} course has been changed to {viewModel.Status}";

                return RedirectToAction("CourseManagement");
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult LastAddedCourses()
        {
            IEnumerable<Course> courses = _courseService.GetCourses(c => c.Status == CourseStatus.Active)
                                                        .OrderByDescending(c => c.DateAdded).Take(5);
            if (!courses.Any())
                return StatusCode(204);
            ViewBag.CarouselId = "last-added-courses-carousel";

            return PartialView("_CarouselCoursesPartial", Mapper.Map<IEnumerable<CourseVm>>(courses));
        }

        [HttpGet]
        public IActionResult BestRatedCourses()
        {
            IEnumerable<Course> courses = _courseService.GetCourses(c => c.Status == CourseStatus.Active)
                                                        .OrderByDescending(c => c.AvgRating).Take(5);
            if (!courses.Any())
                return StatusCode(204);
            ViewBag.CarouselId = "best-rated-courses-carousel";

            return PartialView("_CarouselCoursesPartial", Mapper.Map<IEnumerable<CourseVm>>(courses));
        }
    }
}
