using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class EditUserVm
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Display(Name = "Fullname")]
        public string FullName { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Is active")]
        public bool IsActive { get; set; }

        [Display(Name = "Registration date")]
        public DateTimeOffset RegistrationDate { get; set; }

        [Display(Name = "Fail login attempts lockout end")]
        public DateTimeOffset? LockoutEnd { get; set; }

        [Display(Name = "Last login date")]
        public DateTimeOffset? LastLoginDate { get; set; }

        public IList<string> Roles { get; set; }
    }
}
