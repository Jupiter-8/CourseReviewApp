using System.Collections.Generic;

namespace CourseReviewApp.Model.DataModels
{
    public class CourseOwner : AppUser
    {
        public string BrandName { get; set; }
        public string WebsiteUrl { get; set; }
        public bool CourseInfoEmailsEnabled { get; set; }
        public virtual IList<Course> Courses { get; set; }
        public virtual IList<OwnerComment> OwnerComments { get; set; }
    }
}