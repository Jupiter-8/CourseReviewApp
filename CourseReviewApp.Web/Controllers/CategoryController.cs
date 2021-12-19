﻿using AutoMapper;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using CourseReviewApp.Web.Services.Interfaces;
using CourseReviewApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IEmailSenderService _emailSenderService;

        public CategoryController(ILogger logger, IMapper mapper, ICategoryService categoryService, UserManager<AppUser> userManager, IEmailSenderService emailSenderService)
            : base(logger, mapper, userManager)
        {
            _categoryService = categoryService;
            _emailSenderService = emailSenderService;
        }

        [HttpGet]
        public IActionResult CategoryManagement()
        {
            IEnumerable<CategoryVm> categoryVms = Mapper.Map<IEnumerable<CategoryVm>>(_categoryService.GetCategories());
            return View(categoryVms);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditCategory(int? id = null)
        {
            IEnumerable<Category> parentCategories = _categoryService.GetCategories(c => !c.ParentCategoryId.HasValue);
            List<SelectListItem> categoriesSelect = parentCategories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            ViewBag.ParentCategories = categoriesSelect;

            if (id.HasValue && id.Value != 0)
            {
                Category category = await _categoryService.GetCategory(c => c.Id == id);
                if (category == null)
                    throw new InvalidOperationException($"Category with id: {id} not found.");

                AddOrEditCategoryVm viewModel = Mapper.Map<AddOrEditCategoryVm>(category);
                TempData["PreviousCategoryState"] = JsonSerializer.Serialize(viewModel);

                return View(viewModel);
            }

            return View(new AddOrEditCategoryVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditCategory(AddOrEditCategoryVm viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Id.HasValue && JsonSerializer.Serialize(viewModel) == TempData["PreviousCategoryState"].ToString())
                {
                    TempData["CategoryManagementMsgModal"] = "Category has not been edited.";
                    return RedirectToAction("CategoryManagement");
                }

                await _categoryService.AddOrEditCategory(Mapper.Map<Category>(viewModel));
                TempData["CategoryManagementMsgModal"] = viewModel.Id.HasValue ?
                    "Category has been edited." : $"The {viewModel.Name} category has been added.";

                return RedirectToAction("CategoryManagement");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category category = await _categoryService.GetCategory(c => c.Id == id);
            if (category == null)
                throw new InvalidOperationException($"Category with id: {id} not found.");

            return View(Mapper.Map<CategoryVm>(category));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(CategoryVm viewModel)
        {
            if (ModelState.IsValid)
            {
                Category category = await _categoryService.GetCategory(c => c.Id == viewModel.Id);
                if (category == null)
                    throw new InvalidOperationException($"Category with id: {viewModel.Id} not found.");

                List<Course> courses = category.ParentCategoryId.HasValue ? category.Courses.ToList()
                    : category.SubCategories.SelectMany(sc => sc.Courses).ToList();
                List<string> recipients = courses.Select(c => c.Owner.Email).Distinct().ToList();

                await _categoryService.DeleteCategory(viewModel.Id);
                if (recipients.Any())
                {
                    await _emailSenderService.SendDefaultMessageEmailAsync("Deleted category",
                        $"Category: {viewModel.Name} has been deleted. Now your courses which were assigned to it are available in category:" +
                        $"Not assigned to category. Please log in to your account and choose a new category for your courses.", null, recipients);
                }
                TempData["CategoryManagementMsgModal"] = $"The {viewModel.Name} category has been deleted.";

                return RedirectToAction("CategoryManagement");
            }

            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetSubCategories(int id)
        {
            IEnumerable<Category> categories = _categoryService.GetCategories(c => c.ParentCategoryId == id);
            var subCategories = categories.Select(c => new { id = c.Id, name = c.Name }).ToList();

            return Json(subCategories);
        }
    }
}
