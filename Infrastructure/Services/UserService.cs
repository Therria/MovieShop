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
        public Task<PurchaseReponseModel> GetAllPurchasesForUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseDetailsModel> GetPurchasesDetails(int userId, int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId)
        {
            throw new NotImplementedException();
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
