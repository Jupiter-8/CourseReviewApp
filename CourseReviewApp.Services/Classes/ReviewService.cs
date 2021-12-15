using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
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
        public ReviewService(AppDbContext dbContext, ILogger logger, IMapper mapper)
            : base(dbContext, logger, mapper)
        {
        }

        public async Task<Review> GetReview(Expression<Func<Review, bool>> filter)
        {
            try
            {
                if (filter == null)
                    throw new ArgumentNullException("Filter is null.");
                Review review = await DbContext.Reviews.FirstOrDefaultAsync(filter);

                return review;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<Review> GetReviews(Expression<Func<Review, bool>> filter = null)
        {
            try
            {
                IQueryable<Review> reviews = DbContext.Reviews.AsQueryable();
                if (filter != null)
                    reviews = reviews.Where(filter);

                return reviews;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task AddOrEditReview(Review review)
        {
            try
            {
                if (review == null)
                    throw new ArgumentNullException("Model is null.");

                bool courseExists = await DbContext.Courses.AnyAsync(c => c.Id == review.CourseId);
                if (!courseExists)
                    throw new KeyNotFoundException($"Course with id: {review.CourseId} not found.");
                bool userExists = await DbContext.Users.AnyAsync(u => u.Id == review.AuthorId);
                if (!userExists)
                    throw new KeyNotFoundException($"User with id: {review.AuthorId} not found.");

                bool reviewExists = await DbContext.Reviews.AnyAsync(r =>
                    r.CourseId == review.CourseId && r.AuthorId == review.AuthorId);

                if (review.Id == 0)
                {
                    if (!reviewExists)
                    {
                        review.DateAdded = DateTime.Now;
                        await DbContext.Reviews.AddAsync(review);
                    }
                    else
                        throw new ArgumentException("User's review for this course already exists.");
                }
                else
                {
                    review.DateEdited = DateTime.Now;
                    DbContext.Reviews.Update(review);
                }

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteReview(int id)
        {
            try
            {
                Review review = await DbContext.Reviews
                    .Include(r => r.OwnerComment)
                    .Include(r => r.ReviewReports)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (review == null)
                    throw new KeyNotFoundException($"Review with id: {id} not found.");
                DbContext.Reviews.Remove(review);

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> VoteForReviewHelpfullness(int userId, int reviewId)
        {
            try
            {
                HelpfullReview helpfullReview = await DbContext.HelpfullReviews
                    .FirstOrDefaultAsync(hr => hr.UserId == userId && hr.ReviewId == reviewId);

                if (helpfullReview == null)
                {
                    bool isUserReview = await DbContext.Reviews.AnyAsync(r => r.Id == reviewId && r.AuthorId == userId);
                    if(isUserReview)
                        throw new KeyNotFoundException("User can't vote for his own review.");
                    bool userExists = await DbContext.Users.AnyAsync(u => u.Id == userId);
                    if (!userExists)
                        throw new KeyNotFoundException($"User with id: {userId} not found.");
                    bool reviewExists = await DbContext.Reviews.AnyAsync(r => r.Id == reviewId);
                    if (!reviewExists)
                        throw new KeyNotFoundException($"Review with id: {reviewId} not found.");

                    helpfullReview = new HelpfullReview()
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
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task AddOrEditOwnerComment(OwnerComment ownerComment)
        {
            try
            {
                if (ownerComment == null)
                    throw new ArgumentNullException("Model is null.");
                bool reviewExists = await DbContext.Reviews.AnyAsync(r => r.Id == ownerComment.ReviewId);
                if (!reviewExists)
                    throw new KeyNotFoundException($"Review with id: {ownerComment.ReviewId} not found.");
                bool userExists = await DbContext.Users.AnyAsync(u => u.Id == ownerComment.AuthorId);
                if (!userExists)
                    throw new KeyNotFoundException($"User with id: {ownerComment.AuthorId} not found.");

                bool recordExists = await DbContext.OwnerComments.AnyAsync(or =>
                    or.ReviewId == ownerComment.ReviewId && or.AuthorId == ownerComment.AuthorId);

                if (ownerComment.Id == 0)
                {
                    if (!recordExists)
                    {
                        ownerComment.DateAdded = DateTime.Now;
                        await DbContext.OwnerComments.AddAsync(ownerComment);
                    }
                    else
                        throw new ArgumentException("Owner's comment for this review already exists.");
                }
                else
                {
                    ownerComment.DateEdited = DateTime.Now;
                    DbContext.OwnerComments.Update(ownerComment);
                }

                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<OwnerComment> GetOwnerComment(Expression<Func<OwnerComment, bool>> filter)
        {
            try
            {
                if (filter == null)
                    throw new ArgumentNullException("Filter is null.");
                OwnerComment ownerComment = await DbContext.OwnerComments.FirstOrDefaultAsync(filter);

                return ownerComment;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteOwnerComment(int id)
        {
            try
            {
                OwnerComment ownerComment = await DbContext.OwnerComments.FirstOrDefaultAsync(or => or.Id == id);
                if (ownerComment == null)
                    throw new KeyNotFoundException($"Owner's comment with id: {id} not found.");
                DbContext.OwnerComments.Remove(ownerComment);

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
