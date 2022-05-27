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
            var purchases = await _dbContext.Purchases.Include(p => p.Movie)
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

        public async Task<List<Movie>> GetTop30PurchasedMovies(DateTime fromDate, DateTime toDate)
        {
            var topPurchasedIds = await _dbContext.Purchases.Include(p => p.Movie)
                .Where(p => p.PurchaseDateTime >= fromDate && p.PurchaseDateTime <= toDate)
                .GroupBy(p => p.MovieId)
                .Select(m => new
                {
                    MovieId = m.Key,
                    PurchaseCount = m.Count()
                })
                .OrderByDescending(m => m.PurchaseCount)
                .Select(m => m.MovieId)
                .Take(30)
                .ToListAsync();

            if (topPurchasedIds == null || !topPurchasedIds.Any())
            {
                topPurchasedIds = await _dbContext.Purchases.Include(p => p.Movie)
                .Where(p => p.PurchaseDateTime >= DateTime.Today.AddDays(-90) && p.PurchaseDateTime <= DateTime.Today)
                .GroupBy(p => p.MovieId)
                .Select(m => new
                {
                    MovieId = m.Key,
                    PurchaseCount = m.Count()
                })
                .OrderByDescending(m => m.PurchaseCount)
                .Select(m => m.MovieId)
                .Take(30)
                .ToListAsync();
            }
            var movies = await _dbContext.Movies.Where(m => topPurchasedIds.Contains(m.Id)).ToListAsync();
            return movies;

        }
    }
}
