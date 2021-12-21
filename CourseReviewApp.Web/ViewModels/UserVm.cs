using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class UserVm
    {
        public int Id { get; set; }
        public string AvatarPath { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public bool CourseInfoEmailsEnabled { get; set; }
        public bool ReviewInfoEmailsEnabled { get; set; }
        public IList<string> Roles { get; set; }

        [Display(Name = "Fail login attempts lockout")]
        public bool IsLockedOut => LockoutEnd.HasValue && LockoutEnd.Value >= DateTime.UtcNow;

        [Display(Name = "Brand name")]
        public string BrandName { get; set; }

        [Display(Name = "Website URL")]
        public string WebsiteUrl { get; set; }

        [Display(Name = "Firstname")]
        public string FirstName { get; set; }

        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Display(Name = "Registration date")]
        public DateTimeOffset RegistrationDate { get; set; }

        [Display(Name = "Last login date")]
        public DateTimeOffset? LastLoginDate { get; set; }

        [Display(Name = "Fail login attempts lockout end")]
        public DateTimeOffset? LockoutEnd { get; set; }

        [Display(Name = "Is active")]
        public bool IsActive { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Reviews count")]
        public int CoursesCount { get; set; } 

        [Display(Name = "Reviews count")]
        public int ReviewsCount { get; set; }

        [Display(Name = "User reviews was helpfull")]
        public int UserWasHelpfullCount { get; set; }

        [Display(Name = "Avg courses rating")]
        public double AvgCoursesRating { get; set; }

        [Display(Name = "Courses reviews count")]
        public int CoursesReviewsCount { get; set; }

        [Display(Name = "Is email confirmed")]
        public virtual bool EmailConfirmed { get; set; }
    }
}
