namespace CourseReviewApp.Web.ViewModels
{
    public class DeleteReviewVm : ReviewVm
    {
        public string ReturnUrl { get; set; }
        public bool IsModeratingDeletion { get; set; }
        public bool ReturnToReportManagement { get; set; }
    }
}
