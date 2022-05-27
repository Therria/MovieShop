using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(int id);
        Task<User> GetById(int id);




        Task<bool> AddUserRole(int userId, int roleId = 2);
        Task<bool> AddRole(string roleName);
        Task<List<UserRole>> GetUserRole(int id);




        Task<List<Review>> GetReviewsByUserId(int userId);
        Task<Review> GetReviewByUserIdAndMovieId(int userId, int movieId);
        Task<bool> AddReview(int userId, int movieId, decimal rating, string reviewText);
        Task<bool> UpdateReview(int userId, int movieId, decimal rating, string reviewText);
        Task<bool> DeleteReview(int userId, int movieId);


        Task<List<Favorite>> GetFavoriteByUserId(int userId);
        Task<Favorite> GetFavoriteByUserIdAndMovieId(int userId, int movieId);
        Task<bool> AddFavorite(int userId, int movieId);
        Task<bool> DeleteFavorite(int userId, int movieId);
    }
}
