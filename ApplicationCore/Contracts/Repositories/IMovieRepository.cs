using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<List<Movie>> GetTop30GrossingMovies();

        Task<List<Movie>> GetTop30RatedMovies();

        // multiple return types : tuple
        // Task<(List<Movie>, int totalCount, int totalPages)> GetMoviesByGenres(int genreId, int pageSize = 30, int pageNumber = 1);

        Task<PagedResultSet<Movie>> GetMoviesByGenres(int genreId, int pageSize = 30, int pageNumber = 1);

        Task<List<Genre>> GetGenreList();
        Task<PagedResultSet<Review>> GetReviews(int id, int pageSize = 30, int pageNumber = 1);

        Task<PagedResultSet<Movie>> GetMovies(int pageSize = 30, int pageNumber = 1);


        Task<bool> AddMovieGenre(int movieId, int genreId);
        //Task<Genre> GetGenreByName(string name);
        Task<Genre> GetGenreById(int id);
        Task<MovieGenre> GetMovieGenreByMovieIdAndGenreId(int movieId, int genreId);
    }
}
