using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class CourseFullDetailsVm : CourseLessDetailsVm
    {
        public int Duration { get; set; }
        public string Language { get; set; }
        public string LongDescription { get; set; }
        public IList<ReviewVm> Reviews { get; set; }
        public IList<LearningSkillVm> LearningSkills { get; set; }

        [Display(Name = "Last edited")]
        public DateTimeOffset? DateEdited { get; set; }

        [Display(Name = "Course website")]
        public string CourseWebsiteUrl { get; set; }
    }
}
