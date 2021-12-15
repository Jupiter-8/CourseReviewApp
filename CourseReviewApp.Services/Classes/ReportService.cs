using AutoMapper;
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
    public class ReportService : BaseService, IReportService
    {
        public ReportService(AppDbContext dbContext, ILogger logger, IMapper mapper)
            : base(dbContext, logger, mapper)
        {
        }

        public async Task<ReviewReport> GetReviewReport(Expression<Func<ReviewReport, bool>> filter)
        {
            try
            {
                if (filter == null)
                    throw new ArgumentNullException("Filter is null.");
                ReviewReport reviewReport = await DbContext.ReviewReports.FirstOrDefaultAsync(filter);

                return reviewReport;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<ReviewReport> GetReviewReports(Expression<Func<ReviewReport, bool>> filter = null)
        {
            try
            {
                IQueryable<ReviewReport> reviewReports = DbContext.ReviewReports.AsQueryable();
                if (filter != null)
                    reviewReports = reviewReports.Where(filter);

                return reviewReports;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ReportReview(ReviewReport reviewReport)
        {
            try
            {
                bool reviewExists = await DbContext.Reviews.AnyAsync(r => r.Id == reviewReport.ReviewId);
                if (!reviewExists)
                    throw new KeyNotFoundException($"Review with id: {reviewReport.ReviewId} not found.");
                bool userExists = await DbContext.Users.AnyAsync(u => u.Id == reviewReport.ReportingUserId);
                if (!userExists)
                    throw new KeyNotFoundException($"User with id: {reviewReport.ReportingUserId} not found.");
                bool reportExists = await DbContext.ReviewReports.
                    AnyAsync(rr => rr.ReportingUserId == reviewReport.ReportingUserId && rr.ReviewId == reviewReport.ReviewId);
                if (reportExists)
                    throw new ArgumentException("User's report for this review already exists");

                reviewReport.DateAdded = DateTime.Now;
                await DbContext.ReviewReports.AddAsync(reviewReport);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteReviewReport(int id)
        {
            try
            {
                ReviewReport reviewReport = await DbContext.ReviewReports.FirstOrDefaultAsync(rr => rr.Id == id);

                if (reviewReport == null)
                    throw new KeyNotFoundException($"Review report with id: {id} not found.");
                DbContext.ReviewReports.Remove(reviewReport);

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
