using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CourseReviewApp.Web.ViewModels
{
    public class CategoryVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public IList<CategoryVm> SubCategories { get; set; }
        public int SubCategoriesCount => SubCategories.Count;
        public int SubCategoriesCoursesCount => SubCategories.Sum(sc => sc.CoursesCount);

        [Display(Name = "Parent category")]
        public string ParentCategoryName { get; set; }

        [Display(Name = "Courses")]
        public int CoursesCount { get; set; }

        [Display(Name = "Active courses")]
        public int ActiveCoursesCount { get; set; }
    }
}
