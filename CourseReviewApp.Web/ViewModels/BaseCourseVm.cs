using CourseReviewApp.Model.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class BaseCourseVm
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public CourseStatus Status { get; set; }

        [Display(Name = "Reviews")]
        public int ReviewsCount { get; set; }

        [Display(Name = "Owner")]
        public string OwnerFullName { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Date added")]
        public DateTimeOffset DateAdded { get; set; }

        [Display(Name = "Avg rating")]
        public double AvgRating { get; set; }
    }
}
