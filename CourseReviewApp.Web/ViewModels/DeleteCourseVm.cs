﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class DeleteCourseVm : BaseCourseVm
    {
        public string Language { get; set; }
        public bool OwnerHasCourseInfoEmailsEnabled { get; set; }
        public string OwnerEmail { get; set; }
        public string ImagePath { get; set; }

        [Display(Name = "Short description")]
        public string ShortDescription { get; set; }

        [Display(Name = "Date edited")]
        public DateTimeOffset? DateEdited { get; set; }

        [Display(Name = "Course website")]
        public string CourseWebsiteUrl { get; set; }

        [Display(Name = "Owner name")]
        public string OwnerName { get; set; }
    }
}
