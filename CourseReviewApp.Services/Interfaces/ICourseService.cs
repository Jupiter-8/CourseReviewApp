using CourseReviewApp.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseReviewApp.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Course> GetCourse(Expression<Func<Course, bool>> filter);
        IEnumerable<Course> GetCourses(Expression<Func<Course, bool>> filter = null);
        Task<int> GetCoursesCount();
        Task AddOrEditCourse(Course course);
        Task ChangeCourseStatus(int id, CourseStatus status);
        Task DeleteCourse(int id);
    }
}
