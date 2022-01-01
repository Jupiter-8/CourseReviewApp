using CourseReviewApp.Model.DataModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class AddOrEditReviewVm
    {
        public int? Id { get; set; }
        public int AuthorId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } 
        public DateTimeOffset DateAdded { get; set; }
        public DateTimeOffset? DateEdited { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "{0} review must be between {2} and {1} characters long", MinimumLength = 10)]
        public string Contents { get; set; }

        [Required]
        [Display(Name = "Rating")]
        public RatingValue RatingValue { get; set; }
    }
}
