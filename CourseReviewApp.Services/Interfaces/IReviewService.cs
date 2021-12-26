using CourseReviewApp.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseReviewApp.Services.Interfaces
{
    public interface IReviewService
    {
        Task<Review> GetReview(Expression<Func<Review, bool>> filter);
        Task<IEnumerable<Review>> GetReviews(Expression<Func<Review, bool>> filter = null);
        Task<IEnumerable<Review>> GetLastAddedReviews(int numberOfReviews);
        Task AddOrEditReview(Review review);
        Task DeleteReview(int reviewId);
        Task<bool> VoteForReviewHelpfullness(int userId, int reviewId);
        Task AddOrEditOwnerComment(OwnerComment ownerComment);
        Task<OwnerComment> GetOwnerComment(Expression<Func<OwnerComment, bool>> filter);
        Task DeleteOwnerComment(int commentId);
    }
}
