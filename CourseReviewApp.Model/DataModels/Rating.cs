using System;

namespace CourseReviewApp.Model.DataModels
{
    public abstract class Rating
    {
        public int Id { get; set; }
        public string Contents { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public DateTimeOffset? DateEdited { get; set; } 
        public int AuthorId { get; set; }
    }
}
