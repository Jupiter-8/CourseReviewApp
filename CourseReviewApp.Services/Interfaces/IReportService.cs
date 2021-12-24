using CourseReviewApp.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseReviewApp.Services.Interfaces
{
    public interface IReportService
    {
        Task<ReviewReport> GetReviewReport(Expression<Func<ReviewReport, bool>> filter);
        Task<IEnumerable<ReviewReport>> GetReviewReports(Expression<Func<ReviewReport, bool>> filter = null);
        Task AddReviewReport(ReviewReport reviewReport);
        Task DeleteReviewReport(int reportId);
    }
}
