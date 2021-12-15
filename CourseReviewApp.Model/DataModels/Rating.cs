using System;

namespace CourseReviewApp.Model.DataModels
{
    public abstract class Rating
    {
        public int Id { get; set; }
        public string Contents { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateEdited { get; set; } 
        public int AuthorId { get; set; }
    }
}
