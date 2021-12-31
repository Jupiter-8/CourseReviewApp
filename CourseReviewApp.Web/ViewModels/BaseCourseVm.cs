using CourseReviewApp.Model.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class BaseCourseVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CourseStatus Status { get; set; }
        public UserVm Owner { get; set; }
        public int ReviewsCount { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Date added")]
        public DateTimeOffset DateAdded { get; set; }

        [Display(Name = "Avg rating")]
        public double AvgRating { get; set; }
    }
}
