using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace CourseReviewApp.Services.Classes
{
    public class CourseService : BaseService, ICourseService
    {
        public CourseService(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Course> GetCourse(Expression<Func<Course, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Filter is null.");

            return await DbContext.Courses.FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<Course>> GetCourses(
            Expression<Func<Course, bool>> filter = null)
        {
            IQueryable<Course> courses = DbContext.Courses.AsQueryable();
            if (filter != null)
                courses = courses.Where(filter);

            return await courses.ToListAsync();
        }

        public async Task<int> GetCoursesCount(Expression<Func<Course, bool>> filter = null)
            => filter == null ? await DbContext.Courses.CountAsync() 
                : await DbContext.Courses.CountAsync(filter);

        public async Task<IEnumerable<Course>> GetLastAddedCourses(int numberOfCourses)
            => await DbContext.Courses.Where(c => c.Status == CourseStatus.Active).OrderByDescending(c => c.DateAdded).Take(numberOfCourses).ToListAsync();

        public async Task<IEnumerable<Course>> GetBestRatedCourses(int numberOfCourses)
        {
            IQueryable<Course> courses = DbContext.Courses.Where(c => c.Status == CourseStatus.Active).OrderByDescending(c 
                => Math.Round(c.Reviews.Average(r => (int)r.RatingValue), 1)).Take(numberOfCourses);

            return await courses.ToListAsync();
        }
        
        public async Task AddOrEditCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException("Model is null.");
            if (!await DbContext.Categories.AnyAsync(c => c.Id == course.CategoryId))
                throw new InvalidOperationException($"Category with id: {course.CategoryId} not found.");
            if (!await DbContext.Users.AnyAsync(u => u.Id == course.OwnerId))
                throw new InvalidOperationException($"User with id: {course.OwnerId} not found.");

            for (int i = course.LearningSkills.Count - 1; i >= 0; i--)
            {
                if (course.Id == 0 && string.IsNullOrEmpty(course.LearningSkills[i].Name))
                {
                    course.LearningSkills.RemoveAt(i);
                }
                else if (course.Id != 0 && string.IsNullOrEmpty(course.LearningSkills[i].Name))
                {
                    if (course.LearningSkills[i].CourseId == 0)
                        course.LearningSkills.RemoveAt(i);
                }
            }

            course.Status = CourseStatus.Pending;
            if (course.Id == 0)
            {
                course.DateAdded = DateTimeOffset.Now;
                await DbContext.Courses.AddAsync(course);
            }
            else
            {
                course.DateEdited = DateTimeOffset.Now;
                List<LearningSkill> learningSkills = course.LearningSkills.Where(ls => ls.Name == null).ToList();
                DbContext.LearningSkills.RemoveRange(learningSkills);
                DbContext.Courses.Update(course);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task ChangeCourseStatus(int courseId, CourseStatus status)
        {
            Course course = await DbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null)
                throw new InvalidOperationException($"Course with id {courseId} not found.");
            course.Status = status;

            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteCourse(int courseId)
        {
            Course course = DbContext.Courses
                    .Include(c => c.Reviews)
                        .ThenInclude(r => r.OwnerComment)
                    .Include(c => c.Reviews)
                        .ThenInclude(r => r.ReviewReports)
                    .FirstOrDefault(c => c.Id == courseId);

            if (course == null)
                throw new InvalidOperationException($"Course with id: {courseId} not found.");
            DbContext.Courses.Remove(course);

            await DbContext.SaveChangesAsync();
        }

        public async Task AddCourseToObservedList(int userId, int courseId)
        {
            if (await DbContext.ObservedCourses.AnyAsync(oc => oc.UserId == userId && oc.CourseId == courseId))
                throw new InvalidOperationException($"User with id: {userId} is already observing the course with id: {courseId}.");
            if (!await DbContext.Users.AnyAsync(u => u.Id == userId))
                throw new InvalidOperationException($"User with id: {userId} not found.");
            if (!await DbContext.Courses.AnyAsync(c => c.Id == courseId))
                throw new InvalidOperationException($"Course with id: {courseId} not found.");

            ObservedCourse observedCourse = new()
            {
                UserId = userId,
                CourseId = courseId
            };

            await DbContext.ObservedCourses.AddAsync(observedCourse);
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveCourseFromObservedList(int userId, int courseId)
        {
            ObservedCourse observedCourse = await DbContext.ObservedCourses
                    .FirstOrDefaultAsync(oc => oc.UserId == userId && oc.CourseId == courseId);
            if (observedCourse == null)
                throw new InvalidOperationException($"User with id: {userId} is not observing the course with id: {courseId}.");

            DbContext.ObservedCourses.Remove(observedCourse);
            await DbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ObservedCourse>> GetObservedCourses(int userId)
            => await DbContext.ObservedCourses.Where(oc => oc.UserId == userId).ToListAsync();

        public async Task<IEnumerable<string>> GetObservingUsersEmails(int courseId, int currentUserId)
        {
            Course course = await DbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null)
                throw new InvalidOperationException($"Course with id {courseId} not found.");

            return course.ObservingUsers.Where(oc => oc.UserId != currentUserId).Select(oc => oc.User.Email);
        }
    }
}
