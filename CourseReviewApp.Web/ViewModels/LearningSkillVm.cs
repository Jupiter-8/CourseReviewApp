using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class LearningSkillVm
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        [StringLength(40, ErrorMessage = "{0} must be between {2} and {1} characters long", MinimumLength = 5)]
        public string Name { get; set; }
    }
}
