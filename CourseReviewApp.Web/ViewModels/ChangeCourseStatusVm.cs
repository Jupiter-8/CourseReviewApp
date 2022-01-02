using CourseReviewApp.Model.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class ChangeCourseStatusVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerEmail { get; set; }
        public bool OwnerHasCourseInfoEmailsEnabled { get; set; }

        [Required]
        public CourseStatus Status { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Owner")]
        public string OwnerName { get; set; }
    }
}
