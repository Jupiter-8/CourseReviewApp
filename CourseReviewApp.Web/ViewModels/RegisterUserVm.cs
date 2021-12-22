using System;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class RegisterUserVm
    {
        [Required]
        [RegularExpression("[A-Za-z][A-Za-z0-9_]{5,19}", 
            ErrorMessage = "The username must be at least 6 and at max 20 characters, it must starts with a letter. Digits and the _ symbol are allowed.")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [StringLength(32, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Lastname { get; set; }

        [Display(Name = "Website URL")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        public string WebsiteUrl { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Brandname { get; set; }

        [Required(ErrorMessage = "You must choose the user type.")]
        [Display(Name = "User type")]
        public int UserType { get; set; }

        [Required]
        [Range(1, 1, ErrorMessage = "You must agree to the Terms and Conditions.")]
        public int TermsAccepted { get; set; }
    }
}
