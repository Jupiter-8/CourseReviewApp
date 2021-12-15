﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseReviewApp.Services.Classes
{
    public class CourseService : BaseService, ICourseService
    {
        public CourseService(AppDbContext dbContext, ILogger logger, IMapper mapper)
            : base(dbContext, logger, mapper)
        {
        }

        public async Task<Course> GetCourse(Expression<Func<Course, bool>> filter)
        {
            try
            {
                if (filter == null)
                    throw new ArgumentNullException("Filter is null.");
                Course course = await DbContext.Courses.FirstOrDefaultAsync(filter);

                return course;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<Course> GetCourses(Expression<Func<Course, bool>> filter = null)
        {
            try
            {
                IQueryable<Course> courses = DbContext.Courses.AsQueryable();
                if (filter != null)
                    courses = courses.Where(filter);

                return courses;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<int> GetCoursesCount()
        {
            try
            {
                return await DbContext.Courses.CountAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task AddOrEditCourse(Course course)
        {
            try
            {
                if (course == null)
                    throw new ArgumentNullException("Model is null.");
                bool categoryExists = await DbContext.Categories.AnyAsync(c => c.Id == course.CategoryId);
                if (!categoryExists)
                    throw new KeyNotFoundException($"Category with id: {course.CategoryId} not found.");
                bool userExists = await DbContext.Users.AnyAsync(u => u.Id == course.OwnerId);
                if (!userExists)
                    throw new KeyNotFoundException($"User with id: {course.OwnerId} not found.");

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
                    course.DateAdded = DateTime.Now;
                    await DbContext.Courses.AddAsync(course);
                }
                else
                {
                    course.DateEdited = DateTime.Now;
                    List<LearningSkill> learningSkills = course.LearningSkills.Where(ls => ls.Name == null).ToList();
                    DbContext.LearningSkills.RemoveRange(learningSkills);
                    DbContext.Courses.Update(course);
                }

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ChangeCourseStatus(int id, CourseStatus status)
        {
            try
            {
                Course course = await DbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
                if (course == null)
                    throw new KeyNotFoundException($"Course with id {id} not found.");
                course.Status = status;

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteCourse(int id)
        {
            try
            {
                Course course = DbContext.Courses
                    .Include(c => c.Reviews)
                        .ThenInclude(r => r.OwnerComment)
                    .Include(c => c.Reviews)
                        .ThenInclude(r => r.ReviewReports)
                    .FirstOrDefault(c => c.Id == id);

                if (course == null)
                    throw new KeyNotFoundException($"Course with id: {id} not found.");
                DbContext.Courses.Remove(course);

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
