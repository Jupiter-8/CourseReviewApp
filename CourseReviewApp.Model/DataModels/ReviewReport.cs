using System;

namespace CourseReviewApp.Model.DataModels
{
    public class ReviewReport 
    {
        public int Id { get; set; }
        public string ReportContents { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public ReportReason ReportReason { get; set; }

        public int ReviewId { get; set; }
        public virtual Review Review { get; set; }  
        public int? ReportingUserId { get; set; }
        public virtual AppUser ReportingUser { get; set; } 
    }
}
