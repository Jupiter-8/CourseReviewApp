using CourseReviewApp.Model.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class ChangeCourseStatusVm
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string OwnerEmail { get; set; }

        [Required]
        public int OwnerId { get; set; } 

        [Required]
        public CourseStatus Status { get; set; }

        public bool OwnerHasCourseInfoEmailsEnabled { get; set; }
    }
}
