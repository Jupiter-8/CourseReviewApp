using CourseReviewApp.Model.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class ReportReviewVm
    {
        [Required]
        public int ReviewId { get; set; }

        [Required]
        public int ReportingUserId { get; set; }

        [Required]
        [Display(Name = "Contents")]
        [StringLength(200, ErrorMessage = "Report contents must be between {2} and {1} characters long", MinimumLength = 20)]
        public string ReportContents { get; set; }

        [Required]
        [Display(Name = "Reason")]
        public ReportReason ReportReason { get; set; }
        public ReviewVm Review { get; set; }
    }
}
