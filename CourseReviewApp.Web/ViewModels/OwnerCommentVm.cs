using System;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class OwnerCommentVm
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Contents { get; set; }
        public ReviewVm Review { get; set; }

        [Display(Name = "Author")]
        public string AuthorName { get; set; }

        [Display(Name = "Date added")]
        public DateTimeOffset DateAdded { get; set; }

        [Display(Name = "Date edited")]
        public DateTimeOffset? DateEdited { get; set; }
    }
}
