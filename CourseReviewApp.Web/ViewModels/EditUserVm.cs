using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class EditUserVm
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }

        [Display(Name = "Is active")]
        public bool IsActive { get; set; }

        [Display(Name = "Registration date")]
        public DateTimeOffset RegistrationDate { get; set; }

        [Display(Name = "Fail login attempts lockout end")]
        public DateTimeOffset? LockoutEnd { get; set; }

        [Display(Name = "Last login date")]
        public DateTimeOffset? LastLoginDate { get; set; }
    }
}
