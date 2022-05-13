using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        public List<Movie> GetTop30GrossingMovies()
        {
            // Dapper
            // var top30Movies = movies.GetMovies().OrderByDescending(m => m.Revenue).Take(30);
            var movies = new List<Movie>()
            {
                new Movie {Id = 1, Title ="Inception", PosterUrl ="https://image.tmdb.org/t/p/w342//9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg"},
                new Movie {Id = 2, Title ="Inception", PosterUrl ="https://image.tmdb.org/t/p/w342//9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg"},
                new Movie {Id = 3, Title ="Inception", PosterUrl ="https://image.tmdb.org/t/p/w342//9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg"}
            };

            return movies;
        }
    }
}
