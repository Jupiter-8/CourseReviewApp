using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class ChangeUserStatusVm
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
        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Registration date")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Lockout end")]
        public DateTimeOffset? LockoutEnd { get; set; }

        [Display(Name = "Lockout")]
        public bool IsLockedOut => LockoutEnd.HasValue && LockoutEnd.Value >= DateTime.UtcNow;

        public IList<string> Roles { get; set; }
    }
}
