using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class ObservedCourseVm
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public bool IsCourseActive { get; set; }

        [Display(Name = "Course name")]
        public string CourseName { get; set; }

        [Display(Name = "Course category name")]
        public string CourseCategoryName { get; set; }
    }
}
