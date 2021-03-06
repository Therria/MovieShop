using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repositories
{
    public interface IPurchaseRepository : IRepository<Purchase>
    {
        Task<List<Movie>> GetPurchasesByMovieId(int movieId);
        Task<List<Purchase>> GetPurchasesByUserId(int userId);
        Task<Purchase> GetPurchasesByUserIdAndMovieId(int userId, int movieId);
        Task<List<Movie>> GetTop30PurchasedMovies(DateTime fromDate, DateTime toDate);
    }
}
