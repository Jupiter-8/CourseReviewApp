﻿
namespace CourseReviewApp.Model.DataModels
{
    public class LearningSkill
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
