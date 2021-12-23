using System.Collections.Generic;

namespace CourseReviewApp.Model.DataModels
{
    public class CourseClient : AppUser
    {
        public bool ReviewInfoEmailsEnabled { get; set; }

        public virtual IList<Review> Reviews { get; set; }
        public virtual IList<HelpfullReview> HelpfullReviews { get; set; }
        public virtual IList<ObservedCourse> ObservedCourses { get; set; } 
    }
}
