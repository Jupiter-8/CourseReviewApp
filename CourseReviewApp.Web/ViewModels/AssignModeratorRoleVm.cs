using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class AssignModeratorRoleVm
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Display(Name = "Registration date")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
