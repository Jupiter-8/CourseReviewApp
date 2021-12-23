using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseReviewApp.Services.Classes
{
    public class ReviewService : BaseService, IReviewService
    {
        public ReviewService(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Review> GetReview(Expression<Func<Review, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Filter is null.");

            return await DbContext.Reviews.FirstOrDefaultAsync(filter);
        }

        public IEnumerable<Review> GetReviews(Expression<Func<Review, bool>> filter = null)
        {
            IQueryable<Review> reviews = DbContext.Reviews.AsQueryable();
            if (filter != null)
                reviews = reviews.Where(filter);

            return reviews;
        }

        public async Task AddOrEditReview(Review review)
        {
            if (review == null)
                throw new ArgumentNullException("Model is null.");
            if (!await DbContext.Courses.AnyAsync(c => c.Id == review.CourseId))
                throw new InvalidOperationException($"Course with id: {review.CourseId} not found.");
            if (!await DbContext.Users.AnyAsync(u => u.Id == review.AuthorId))
                throw new InvalidOperationException($"User with id: {review.AuthorId} not found.");

            if (review.Id == 0)
            {
                if (!await DbContext.Reviews.AnyAsync(r =>
                    r.CourseId == review.CourseId && r.AuthorId == review.AuthorId))
                {
                    review.DateAdded = DateTimeOffset.Now;
                    await DbContext.Reviews.AddAsync(review);
                }
                else
                    throw new ArgumentException("User's review for this course already exists.");
            }
            else
            {
                review.DateEdited = DateTimeOffset.Now;
                DbContext.Reviews.Update(review);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteReview(int reviewId)
        {
            Review review = await DbContext.Reviews
                    .Include(r => r.OwnerComment)
                    .Include(r => r.ReviewReports)
                    .FirstOrDefaultAsync(r => r.Id == reviewId);

            if (review == null)
                throw new InvalidOperationException($"Review with id: {reviewId} not found.");
            DbContext.Reviews.Remove(review);

            await DbContext.SaveChangesAsync();
        }

        public async Task<bool> VoteForReviewHelpfullness(int userId, int reviewId)
        {
            HelpfullReview helpfullReview = await DbContext.HelpfullReviews
                    .FirstOrDefaultAsync(hr => hr.UserId == userId && hr.ReviewId == reviewId);

            if (helpfullReview == null)
            {
                if (await DbContext.Reviews.AnyAsync(r => r.Id == reviewId && r.AuthorId == userId))
                    throw new ArgumentException("User can't vote for his own review.");
                if (!await DbContext.Users.AnyAsync(u => u.Id == userId))
                    throw new InvalidOperationException($"User with id: {userId} not found.");
                if (!await DbContext.Reviews.AnyAsync(r => r.Id == reviewId))
                    throw new InvalidOperationException($"Review with id: {reviewId} not found.");

                helpfullReview = new()
                {
                    UserId = userId,
                    ReviewId = reviewId
                };
                await DbContext.HelpfullReviews.AddAsync(helpfullReview);
                await DbContext.SaveChangesAsync();

                return true;
            }
            else
            {
                DbContext.HelpfullReviews.Remove(helpfullReview);
                await DbContext.SaveChangesAsync();

                return false;
            }
        }

        public async Task AddOrEditOwnerComment(OwnerComment ownerComment)
        {
            if (ownerComment == null)
                throw new ArgumentNullException("Model is null.");
            if (!await DbContext.Reviews.AnyAsync(r => r.Id == ownerComment.ReviewId))
                throw new InvalidOperationException($"Review with id: {ownerComment.ReviewId} not found.");
            if (!await DbContext.Users.AnyAsync(u => u.Id == ownerComment.AuthorId))
                throw new InvalidOperationException($"User with id: {ownerComment.AuthorId} not found.");

            if (ownerComment.Id == 0)
            {
                if (!await DbContext.OwnerComments.AnyAsync(or =>
                    or.ReviewId == ownerComment.ReviewId && or.AuthorId == ownerComment.AuthorId))
                {
                    ownerComment.DateAdded = DateTimeOffset.Now;
                    await DbContext.OwnerComments.AddAsync(ownerComment);
                }
                else
                    throw new ArgumentException("Owner's comment for this review already exists.");
            }
            else
            {
                ownerComment.DateEdited = DateTimeOffset.Now;
                DbContext.OwnerComments.Update(ownerComment);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<OwnerComment> GetOwnerComment(Expression<Func<OwnerComment, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("Filter is null.");
            OwnerComment ownerComment = await DbContext.OwnerComments.FirstOrDefaultAsync(filter);

            return ownerComment;
        }

        public async Task DeleteOwnerComment(int commentId)
        {
            OwnerComment ownerComment = await DbContext.OwnerComments.FirstOrDefaultAsync(or => or.Id == commentId);
            if (ownerComment == null)
                throw new InvalidOperationException($"Owner's comment with id: {commentId} not found.");
            DbContext.OwnerComments.Remove(ownerComment);

            await DbContext.SaveChangesAsync();
        }
    }
}
