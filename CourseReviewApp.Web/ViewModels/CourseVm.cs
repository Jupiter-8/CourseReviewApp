using CourseReviewApp.Model.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class CourseVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Language { get; set; }
        public string ImagePath { get; set; }
        public string LongDescription { get; set; }
        public CourseStatus Status { get; set; }
        public UserVm Owner { get; set; }
        public IList<ReviewVm> Reviews { get; set; }
        public IList<LearningSkillVm> LearningSkills { get; set; }
        public IList<ObservedCourseVm> ObservingUsers { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Short description")]
        public string ShortDescription { get; set; }

        [Display(Name = "Date added")]
        public DateTimeOffset DateAdded { get; set; }

        [Display(Name = "Last edited")]
        public DateTimeOffset? DateEdited { get; set; }

        [Display(Name = "Course website")]
        public string CourseWebsiteUrl { get; set; }
         
        [Display(Name = "Avg rating")]
        public double AvgRating { get; set; }
    }
}
