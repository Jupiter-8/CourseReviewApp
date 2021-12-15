
namespace CourseReviewApp.Model.DataModels
{
    public class OwnerComment : Rating
    {
        public int ReviewId { get; set; }
        public virtual Review Review { get; set; }
        public virtual CourseOwner Author { get; set; }
    }
}
