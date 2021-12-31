﻿using System.Collections.Generic;

namespace CourseReviewApp.Web.ViewModels
{
    public class CourseLessDetailsVm : BaseCourseVm
    {
        public string OwnerFullName { get; set; }
        public string ImagePath { get; set; }
        public string ShortDescription { get; set; }
        public IList<ObservedCourseVm> ObservingUsers { get; set; }
    }
}
