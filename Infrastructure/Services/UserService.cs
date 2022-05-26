using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        public UserService(IUserRepository userRepository, IPurchaseRepository purchaseRepository)
        {
            _userRepository = userRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<UserDetailsModel> GetUserDetails(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                return null;
            }
            var userDetails = new UserDetailsModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                HashedPassword = user.HashedPassword,  // hashedpassword + salt -> password ?
                Salt = user.Salt,
                PhoneNumber = user.PhoneNumber,
                LockoutEndDate = user.LockoutEndDate,
                LastLoginDateTime = user.LastLoginDateTime
            };

            foreach (var favorite in userDetails.Favorites)
            {
                userDetails.Favorites.Add(new FavoriteDetailsModel { 
                    Id = favorite.Id, 
                    MovieId = favorite.MovieId, 
                    UserId = favorite.UserId, 
                    Movie = new MovieCardModel 
                    { 
                        Id = favorite.MovieId, 
                        Title = favorite.Movie.Title, 
                        PosterUrl = favorite.Movie.PosterUrl
                    }
                });
            }

            foreach(var review in userDetails.Reviews)
            {
                userDetails.Reviews.Add(new ReviewDetailsModel
                {
                    MovieId = review.MovieId,
                    UserId = review.UserId,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText,
                    Movie = new MovieCardModel 
                    { 
                        Id = review.MovieId,
                        Title = review.Movie.Title,
                        PosterUrl = review.Movie.PosterUrl
                    }
                });
            }

            foreach(var purchase in userDetails.Purchases)
            {
                userDetails.Purchases.Add(new PurchaseDetailsModel
                {
                    Id= purchase.Id,
                    PurchaseDateTime = purchase.PurchaseDateTime,
                    PurchaseNumber = purchase.PurchaseNumber,
                    TotalPrice = purchase.TotalPrice,
                    MovieCard = new MovieCardModel
                    {
                        Id = purchase.MovieCard.Id ,
                        Title = purchase.MovieCard.Title,
                        PosterUrl= purchase.MovieCard.PosterUrl
                    }
                });
            }

            foreach(var role in userDetails.Roles)
            {
                userDetails.Roles.Add(new RoleModel
                {
                    Id = role.Id,
                    Name = role.Name
                });
            }

            return userDetails;

        }

        public async Task<PurchaseReponseModel> GetAllPurchasesForUser(int id)
        {
            var purchases = await _purchaseRepository.GetPurchasesByUserId(id);
            var purchaseReponse = new PurchaseReponseModel()
            {
                UserId = id,
            };

            foreach (var purchase in purchases)
            {
                purchaseReponse.PurchaseDetails.Add(new PurchaseDetailsModel()
                {
                    Id = purchase.Id,
                    PurchaseNumber = purchase.PurchaseNumber,
                    PurchaseDateTime = purchase.PurchaseDateTime,
                    TotalPrice = purchase.TotalPrice,
                    MovieCard = new MovieCardModel()
                    {
                        Id = purchase.MovieId,
                        PosterUrl = purchase.Movie.PosterUrl,
                        Title = purchase.Movie.Title
                    }
                });
            }

            return purchaseReponse;
        }      

        public async Task<PurchaseDetailsModel> GetPurchasesDetails(int userId, int movieId)
        {
            var purchase = await _purchaseRepository.GetPurchasesByUserIdAndMovieId(userId, movieId);
            var purchaseDetails = new PurchaseDetailsModel()
            {
                Id = purchase.Id,
                PurchaseNumber = purchase.PurchaseNumber,
                PurchaseDateTime = purchase.PurchaseDateTime,
                TotalPrice = purchase.TotalPrice,
                MovieCard = new MovieCardModel()
                {
                    Id = movieId,
                    PosterUrl = purchase.Movie.PosterUrl,
                    Title = purchase.Movie.Title
                }
            };

            return purchaseDetails;
        }

        public async Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId)
        {
            var purchase = await _purchaseRepository.GetPurchasesByUserIdAndMovieId(userId, purchaseRequest.MovieId);
            return purchase != null;
        }

        public async Task<bool> IsMoviePurchased(int userId, int movieId)
        {
            var purchase = await _purchaseRepository.GetPurchasesByUserIdAndMovieId(userId, movieId);
            return purchase != null;
        }

        public async Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new Exception("User was not found");
            }

            var purchaseNumber = Guid.NewGuid();

            var purchase = new Purchase
            {
                UserId = user.Id,
                TotalPrice = purchaseRequest.TotalPrice,
                PurchaseDateTime = purchaseRequest.PurchaseDateTime,
                MovieId = purchaseRequest.MovieId,
                PurchaseNumber = purchaseNumber
            };

            var createdPurchase = _purchaseRepository.Add(purchase);

            if (createdPurchase.Id > 0)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            var user = await _userRepository.GetUserById(reviewRequest.UserId);
            if (user == null)
            {
                throw new Exception("User was not found");
            }

            return await _userRepository.UpdateReview(reviewRequest.UserId,
                reviewRequest.MovieId, reviewRequest.Rating, reviewRequest.ReviewText);
        }

        public async Task<bool> AddMovieReview(ReviewRequestModel reviewRequest)
        {
            var user = await _userRepository.GetUserById(reviewRequest.UserId);
            if (user == null)
            {
                throw new Exception("User was not found");
            }

            return await _userRepository.AddReview(reviewRequest.UserId, 
                reviewRequest.MovieId, reviewRequest.Rating, reviewRequest.ReviewText);
        }

        public async Task<bool> DeleteMovieReview(int userId, int movieId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new Exception("User was not found");
            }

            return await _userRepository.DeleteReview(userId, movieId);
        }

        public async Task<ReviewResponseModel> GetAllReviewsByUser(int id)
        {
            var reviews = await _userRepository.GetReviewsByUserId(id);
            var reviewResponse = new ReviewResponseModel()
            {
                UserId = id
            };
            foreach (var review in reviews)
            {
                reviewResponse.ReviewDetails.Add(new ReviewDetailsModel()
                {
                    MovieId = review.MovieId,
                    UserId = review.UserId,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText,
                    Movie = new MovieCardModel
                    {
                        Id = review.MovieId,
                        Title = review.Movie.Title,
                        PosterUrl = review.Movie.PosterUrl
                    }
                });
            }

            return reviewResponse;
        }
    }
}
