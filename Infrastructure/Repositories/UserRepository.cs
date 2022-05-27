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

        public async Task<User> GetById(int id)
        {
            var user = await _dbContext.Users.Include(u => u.Favorites).ThenInclude(u => u.Movie)
                .Include(u => u.Reviews).ThenInclude(u => u.Movie)
                .Include(u => u.Purchases).ThenInclude(u => u.Movie)
                .Include(u => u.UsersOfRole).ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
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
                .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);

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


        public async Task<List<Favorite>> GetFavoriteByUserId(int userId)
        {
            var favorites = await _dbContext.Favorites.Include(f => f.Movie)
                .Where(f => f.UserId == userId).ToListAsync();

            return favorites;
        }

        public async Task<Favorite> GetFavoriteByUserIdAndMovieId(int userId, int movieId)
        {
            var favorite = await _dbContext.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MovieId == movieId);

            return favorite;
        }
        
        public async Task<bool> AddFavorite(int userId, int movieId)
        {
            var favorite = await GetFavoriteByUserIdAndMovieId(userId, movieId);
            if (favorite == null)
            {
                var createdFavorite = new Favorite
                {
                    MovieId = movieId,
                    UserId = userId
                };

                await _dbContext.Favorites.AddAsync(createdFavorite);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            // already favorited
            return false;
        }
        public async Task<bool> DeleteFavorite(int userId, int movieId)
        {
            var favorite = await GetFavoriteByUserIdAndMovieId(userId, movieId);
            if (favorite != null)
            {
                _dbContext.Favorites.Remove(favorite);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            // not been favorited
            return false;
        }


    }
}
