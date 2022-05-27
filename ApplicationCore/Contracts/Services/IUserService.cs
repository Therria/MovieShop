using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApplicationCore.Contracts.Services
{
    public interface IUserService
    {
        Task<UserDetailsModel> GetUserDetails(int id);
        Task<string> GetUserNameById(int Id);
        Task<bool> IsAdmin(int Id);


        Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId);
        Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId);
        Task<bool> IsMoviePurchased(int userId, int movieId);
        Task<PurchaseReponseModel> GetAllPurchasesForUser(int id);
        Task<PurchaseDetailsModel> GetPurchasesDetails(int userId, int movieId);

        Task<bool> AddMovieReview(ReviewRequestModel reviewRequest);
        Task<bool> UpdateMovieReview(ReviewRequestModel reviewRequest);
        Task<bool> DeleteMovieReview(int userId, int movieId);
        Task<ReviewResponseModel> GetAllReviewsForUser(int id);

        Task<bool> AddUserFavorite(FavoriteRequestModal favoriteRequest);
        Task<bool> DeleteUserFavorite(FavoriteRequestModal favoriteRequest);
        Task<bool> IsMovieFavorited(int userId, int movieId);
        Task<FavoriteResponseModel> GetAllFavoritesForUser(int userId);
    }
}
