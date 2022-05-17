using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public List<Movie> GetTop30GrossingMovies()
        {
            // Dapper
            // var top30Movies = movies.GetMovies().OrderByDescending(m => m.Revenue).Take(30);
            var movies = _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToList();

            return movies;
        }
    }
}
