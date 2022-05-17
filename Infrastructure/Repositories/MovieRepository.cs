using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public List<Movie> GetTop30GrossingMovies()
        {

            // var top30Movies = movies.GetMovies().OrderByDescending(m => m.Revenue).Take(30);
            var movies = _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToList();

            return movies;
        }

        public override Movie GetById(int id)
        {
            // Movie m LEFT JOIN MovieGenre mg on m.id = mg.movieid LEFT JOIN Genre g on mg.genreid = g.id  +
            // Movie m LEFT JOIN MovieCast mc on m.id = mc.movieid LEFT JOIN Cast c on mc.castid = c.id +
            // Movie m LEFT JOIN MovieCrew mc on m.id = mc.movieid LEFT JOIN Crew c on mc.crewid = c.id
            var movie = _dbContext.Movies.Include(m => m.MoviesOfGenre).ThenInclude(m => m.Genre)
                .Include(m => m.MoviesOfCast).ThenInclude(m => m.Cast)
                .Include(m => m.MoviesOfCrew).ThenInclude(m => m.Crew)
                .FirstOrDefault(m => m.Id == id);

            return movie;
        }
    }
}
