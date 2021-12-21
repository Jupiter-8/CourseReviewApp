using CourseReviewApp.Model.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class ReviewVm
    {
        public int Id { get; set; } 
        public int AuthorId { get; set; }
        public int CourseId { get; set; }
        public int CourseOwnerId { get; set; }
        public string Contents { get; set; }
        public string CourseName { get; set; }
        public UserVm Author { get; set; }
        public OwnerCommentVm OwnerComment { get; set; }
        public IList<ReviewReportVm> ReviewReports { get; set; }
        public IList<HelpfullReviewVm> HelpfullReviews { get; set; }

        [Display(Name = "Was helpfull")]
        public int WasHelpfullCount { get; set; }

        [Display(Name = "Rating")]
        public RatingValue RatingValue { get; set; }

        [Display(Name = "Date added")]
        public DateTimeOffset DateAdded { get; set; }

        [Display(Name = "Date edited")]
        public DateTimeOffset? DateEdited { get; set; } 
    }
}
