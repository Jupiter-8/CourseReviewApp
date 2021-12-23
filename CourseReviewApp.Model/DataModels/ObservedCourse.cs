namespace CourseReviewApp.Model.DataModels
{
    public class ObservedCourse
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }

        public virtual CourseClient User { get; set; }
        public virtual Course Course { get; set; }
    }
}
