using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseReviewApp.Web.ViewModels
{
    public class AddOrEditCourseVm
    {
        public int? Id { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public DateTimeOffset? DateEdited { get; set; }
        public IFormFile Image { get; set; }
        public string ImagePath { get; set; }
        public bool ImgToDelete { get; set; }
        public int OwnerId { get; set; }
        public int Duration { get; set; }
        public IList<LearningSkillVm> LearningSkills { get; set; }

        [Required]
        [StringLength(70, ErrorMessage = "{0} must be between {2} and {1} characters long", MinimumLength = 10)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Short description")]
        [StringLength(100, ErrorMessage = "Short description must be between {2} and {1} characters long", MinimumLength = 30)]
        public string ShortDescription { get; set; }

        [Required]
        [Display(Name = "Long description")]
        [StringLength(4000, ErrorMessage = "Long description must be between {2} and {1} characters long", MinimumLength = 50)]
        public string LongDescription { get; set; }

        [Required]
        [Display(Name = "Course website URL")]
        [StringLength(2048, ErrorMessage = "Course website URL must be between {2} and {1} characters long", MinimumLength = 1)]
        public string CourseWebsiteUrl { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0} must be between {2} and {1} characters long", MinimumLength = 3)]
        public string Language { get; set; }

        [Required]
        [Display(Name = "Subcategory")]
        public int? CategoryId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int? ParentCategoryId { get; set; }
    }
}
