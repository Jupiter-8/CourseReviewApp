﻿using CourseReviewApp.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseReviewApp.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Course> GetCourse(Expression<Func<Course, bool>> filter);
        Task<IEnumerable<Course>> GetCourses(Expression<Func<Course, bool>> filter = null);
        Task<IEnumerable<Course>> GetLastAddedCourses(int numberOfCourses);
        Task<IEnumerable<Course>> GetBestRatedCourses(int numberOfCourses); 
        Task<int> GetCoursesCount(Expression<Func<Course, bool>> filter = null);
        Task AddOrEditCourse(Course course);
        Task ChangeCourseStatus(int courseId, CourseStatus status);
        Task DeleteCourse(int courseId);
        Task AddCourseToObservedList(int userId, int courseId);
        Task RemoveCourseFromObservedList(int userId, int courseId);
        Task<IEnumerable<ObservedCourse>> GetObservedCourses(int userId);
        Task<IEnumerable<string>> GetObservingUsersEmails(int courseId, int currentUserId);
    }
}
