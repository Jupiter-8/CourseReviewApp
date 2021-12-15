using Microsoft.AspNetCore.Http;

namespace CourseReviewApp.Web.ViewModels
{
    public class AddOrEditUserAvatarVm 
    {
        public bool IsNew { get; set; } = true;
        public IFormFile Image { get; set; }
        public string AvatarPath { get; set; }
    }
}
