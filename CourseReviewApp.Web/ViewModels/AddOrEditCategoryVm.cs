using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class AddOrEditCategoryVm
    {
        public int? Id { get; set; }
        public int? ParentCategoryId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0} must be between {2} and {1} characters long", MinimumLength = 5)]
        public string Name { get; set; }
    }
}
