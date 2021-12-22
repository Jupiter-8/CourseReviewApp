using AutoMapper;
using CourseReviewApp.Services.Interfaces;
using CourseReviewApp.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper Mapper;

        public CategoryViewComponent(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            Mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<CategoryVm> categoryVms = Mapper.Map<IEnumerable<CategoryVm>>(_categoryService.GetCategories());
            return View(categoryVms);
        }
    }
}
