
namespace CourseReviewApp.Model.DataModels
{
    public class HelpfullReview
    {
        public int UserId { get; set; }
        public int ReviewId { get; set; }

        public virtual CourseClient User { get; set; }
        public virtual Review Review { get; set; }
    }
}
