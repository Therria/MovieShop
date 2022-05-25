using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Purchase> GetPurchasesByUserIdAndMovieId(int userId, int movieId)
        {
            var purchases = await _dbContext.Purchases
                .Where(p => p.UserId == userId && p.MovieId == movieId)
                .SingleOrDefaultAsync();

            return purchases;
        }

        public async Task<List<Movie>> GetPurchasesByMovieId(int movieId)
        {
            var movies = await _dbContext.Movies.Include(m => m.Purchases)
                .Where(m => m.Id == movieId)
                .ToListAsync();

            return movies;
        }

        public async Task<List<Purchase>> GetPurchasesByUserId(int userId)
        {
            var purchases = await _dbContext.Purchases.Include(p => p.Movie)
                .Where(p => p.UserId == userId).ToListAsync();

            return purchases;
        }
    }
}
