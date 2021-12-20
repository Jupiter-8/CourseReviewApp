using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class AddOrEditTermsAndConditionsFileVm
    {
        public bool IsNew { get; set; } = true;
        public string FilePath { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
