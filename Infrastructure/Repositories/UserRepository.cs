using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<List<Review>> GetReviewsByUserId(int userId)
        {
            var reviews = await _dbContext.Reviews.Include(p => p.Movie)
                .Where(p => p.UserId == userId).ToListAsync();

            return reviews;
        }

        public async Task<Review> GetReviewByUserIdAndMovieId(int userId, int movieId)
        {
            var review = await _dbContext.Reviews
                .Where(r => r.UserId == userId && r.MovieId == movieId).FirstOrDefaultAsync();

            return review;
        }

        public async Task<bool> AddReview(int userId, int movieId, decimal rating, string reviewText)
        {
            var review = await GetReviewByUserIdAndMovieId(userId, movieId);

            if (review == null)
            {
                var createdReview = new Review
                {
                    UserId = userId,
                    MovieId = movieId,
                    Rating = rating,
                    ReviewText = reviewText
                };
                await _dbContext.Reviews.AddAsync(createdReview);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            
            return false;
        }

        public async Task<bool> UpdateReview(int userId, int movieId, decimal rating, string reviewText)
        {
            var review = await GetReviewByUserIdAndMovieId(userId, movieId);

            if (review != null)
            {
                var getReview = await _dbContext.Reviews.Where(r => r.UserId == userId && r.MovieId == movieId)
                    .SingleOrDefaultAsync();
                if (getReview == null)
                {
                    throw new Exception("Review retrive error");
                }
                getReview.Rating = rating;
                getReview.ReviewText = reviewText;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            
            return false;
        }

        public async Task<bool> DeleteReview(int userId, int movieId)
        {
            var review = await GetReviewByUserIdAndMovieId(userId, movieId);

            if (review != null)
            {
                _dbContext.Reviews.Remove(review);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }





    }
}
