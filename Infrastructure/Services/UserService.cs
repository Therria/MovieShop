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

        public async Task<PurchaseReponseModel> GetAllPurchasesForUser(int id)
        {
            var purchases = await _purchaseRepository.GetPurchasesByUserId(id);
            var purchaseReponse = new PurchaseReponseModel()
            {
                UserId = id,
            };

            foreach (var purchase in purchases)
            {
                var purchaseDetails = new PurchaseDetailsModel()
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
                };
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


    }
}
