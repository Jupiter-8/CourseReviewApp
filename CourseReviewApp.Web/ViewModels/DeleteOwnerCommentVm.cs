namespace CourseReviewApp.Web.ViewModels
{
    public class DeleteOwnerCommentVm : OwnerCommentVm
    {
        public string ReturnUrl { get; set; }
        public bool IsModeratingDeletion { get; set; }
    }
}
