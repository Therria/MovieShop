using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Services
{
    public interface IMovieService
    {
        Task<List<MovieCardModel>> GetTop30GrossingMovies();
        Task<List<MovieCardModel>> GetTop30RatedMovies();
        Task<MovieDetailsModel> GetMovieDetails(int movieId);
        Task<PagedResultSet<MovieCardModel>> GetMoviesByGenrePagination(int genreId, int pageSize = 30, int pageNumber = 1);
        Task<List<GenreModel>> GetGenreList();
    }
}
