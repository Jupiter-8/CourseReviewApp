using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CourseReviewApp.Model.DataModels
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }
        public bool IsActive { get; set; }
        public string AvatarPath { get; set; }
        public bool LockoutMessageSent { get; set; }
        public IList<string> Roles { get; set; }

        public virtual IList<ReviewReport> ReviewReports { get; set; }
    }
}
