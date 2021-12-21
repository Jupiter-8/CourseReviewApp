using System;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class AddOrEditOwnerCommentVm
    {
        public int? Id { get; set; }
        public int CourseId { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public DateTimeOffset? DateEdited { get; set; }
        public ReviewVm Review { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "{0} must be between {2} and {1} characters long", MinimumLength = 10)]
        public string Contents { get; set; }

        [Required]
        public int ReviewId { get; set; }

        [Required]
        public int AuthorId { get; set; }  
    }
}
