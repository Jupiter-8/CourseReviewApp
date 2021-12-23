using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseReviewApp.Model.DataModels
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; } 
        public DateTimeOffset DateAdded { get; set; }
        public DateTimeOffset? DateEdited { get; set; }
        public string CourseWebsiteUrl { get; set; }
        public CourseStatus Status { get; set; }
        public int Duration { get; set; }
        public string Language { get; set; }
        public string ImagePath { get; set; } 

        public int? CategoryId { get; set; } 
        public virtual Category Category { get; set; }
        public int OwnerId { get; set; }
        public virtual CourseOwner Owner { get; set; }
        public virtual IList<Review> Reviews { get; set; }
        public virtual IList<LearningSkill> LearningSkills { get; set; }
        public virtual IList<ObservedCourse> ObservingUsers { get; set; } 
        public double AvgRating 
            => Reviews == null || Reviews.Count == 0 ? 0.0d : Math.Round(Reviews.Average(r => (int)r.RatingValue), 1);
    }
}
