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
    public class ReportService : BaseService, IReportService
    {
        public ReportService(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ReviewReport> GetReviewReport(Expression<Func<ReviewReport, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Filter is null.");
            ReviewReport reviewReport = await DbContext.ReviewReports.FirstOrDefaultAsync(filter);

            return reviewReport;
        }

        public IEnumerable<ReviewReport> GetReviewReports(Expression<Func<ReviewReport, bool>> filter = null)
        {
            IQueryable<ReviewReport> reviewReports = DbContext.ReviewReports.AsQueryable();
            if (filter != null)
                reviewReports = reviewReports.Where(filter);

            return reviewReports;
        }

        public async Task AddReviewReport(ReviewReport reviewReport)
        {
            if (!await DbContext.Reviews.AnyAsync(r => r.Id == reviewReport.ReviewId))
                throw new KeyNotFoundException($"Review with id: {reviewReport.ReviewId} not found.");
            if (!await DbContext.Users.AnyAsync(u => u.Id == reviewReport.ReportingUserId))
                throw new KeyNotFoundException($"User with id: {reviewReport.ReportingUserId} not found.");
            if (await DbContext.ReviewReports.AnyAsync(rr => rr.ReportingUserId == reviewReport.ReportingUserId
               && rr.ReviewId == reviewReport.ReviewId))
                throw new ArgumentException("User's report for this review already exists");

            reviewReport.DateAdded = DateTimeOffset.Now;
            await DbContext.ReviewReports.AddAsync(reviewReport);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteReviewReport(int id)
        {
            ReviewReport reviewReport = await DbContext.ReviewReports.FirstOrDefaultAsync(rr => rr.Id == id);
            if (reviewReport == null)
                throw new KeyNotFoundException($"Review report with id: {id} not found.");
            DbContext.ReviewReports.Remove(reviewReport);

            await DbContext.SaveChangesAsync();
        }
    }
}
