using System.Collections.Generic;

namespace CourseReviewApp.Model.DataModels
{
    public class Review : Rating
    {
        public int CourseId { get; set; }
        public RatingValue RatingValue { get; set; }

        public virtual Course Course { get; set; }
        public virtual OwnerComment OwnerComment { get; set; } 
        public virtual CourseClient Author { get; set; }
        public virtual IList<ReviewReport> ReviewReports { get; set; }
        public virtual IList<HelpfullReview> HelpfullReviews { get; set; }
    }
}
