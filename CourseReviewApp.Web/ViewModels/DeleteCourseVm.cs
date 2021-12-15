using CourseReviewApp.Model.DataModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class DeleteCourseVm
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string OwnerEmail { get; set; }

        [Required]
        public string ImagePath { get; set; }
        
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Short description")]
        public string ShortDescription { get; set; }

        [Display(Name = "Date added")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Date edited")]
        public DateTime? DateEdited { get; set; }

        [Display(Name = "Course website")]
        public string CourseWebsiteUrl { get; set; }

        [Display(Name = "Avg rating")]
        public double AvgRating { get; set; }

        [Display(Name = "Owner name")]
        public string OwnerName { get; set; }

        public string Language { get; set; }
        public bool OwnerHasCourseInfoEmailsEnabled { get; set; }
    }
}
