using System.Collections.Generic;

namespace CourseReviewApp.Model.DataModels
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? ParentCategoryId { get; set; } 
        public virtual Category ParentCategory { get; set; } 
        public virtual IList<Course> Courses { get; set; }
        public virtual IList<Category> SubCategories { get; set; } 
    }
}
