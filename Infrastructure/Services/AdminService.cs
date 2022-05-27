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
    public class AdminService : IAdminService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        public AdminService(IMovieRepository movieRepository, IUserRepository userRepository, IPurchaseRepository purchaseRepository)
        {
            _movieRepository = movieRepository;
            _userRepository = userRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<bool> IsAdmin(int userId)
        {
            var roles = await _userRepository.GetUserRole(userId);
            foreach (var role in roles)
            {
                if (role.Role.Name.Equals("Admin"))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsMovieExisted(int movieId)
        {
            var movie = await _movieRepository.GetById(movieId);
            if (movie != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> CreateMovie(MovieCreateRequestModel request, string userName)
        {
            var checkMovie = await IsMovieExisted(request.Id);
            if (!checkMovie)
            {
                // create movie
                var newMovie = new Movie
                {
                    Id = request.Id,
                    Title = request.Title,
                    Overview = request.Overview,
                    Tagline = request.Tagline,
                    Budget = request.Budget,
                    Revenue = request.Revenue,
                    ImdbUrl = request.ImdbUrl,
                    TmdbUrl = request.TmdbUrl,
                    PosterUrl = request.PosterUrl,
                    BackdropUrl = request.BackdropUrl,
                    OriginalLanguage = request.OriginalLanguage,
                    ReleaseDate = request.ReleaseDate,
                    RunTime = request.RunTime,
                    Price = request.Price,
                    CreatedBy = userName,
                    CreatedDate = DateTime.UtcNow
                };

                var createdMovie = await _movieRepository.Add(newMovie);

                if (createdMovie.Id <= 0)
                {
                    return false;
                }

                // create genres of the new movie
                foreach (var genre in request.Genres)
                {
                    //var genreId = (await _movieRepository.GetGenreByName(genre.Name)).Id;
                    var createdMovieGenre = await _movieRepository.AddMovieGenre(createdMovie.Id, genre.Id);
                    if (!createdMovieGenre)
                    {
                        return false;
                    }
                }
                return true;

            }
            // movie already exists
            return false;
        }

        public async Task<bool> UpdateMovie(MovieUpdateRequestModel request, string userName)
        {
            var checkMovie = await IsMovieExisted(request.Id);
            if (checkMovie)
            {
                // update movie
                var movie = await _movieRepository.GetById(request.Id);
                movie.Title = request.Title != null? request.Title : movie.Title;
                movie.Overview = request.Overview != null ? request.Overview : movie.Overview;
                movie.Tagline = request.Tagline != null ? request.Tagline : movie.Tagline;
                movie.Budget = request.Budget != null ? request.Budget : movie.Budget;
                movie.Revenue = request.Revenue != null ? request.Revenue : movie.Revenue;
                movie.ImdbUrl = request.ImdbUrl != null ? request.ImdbUrl : movie.ImdbUrl;
                movie.TmdbUrl = request.TmdbUrl != null ? request.TmdbUrl : movie.TmdbUrl;
                movie.PosterUrl = request.PosterUrl != null ? request.PosterUrl : movie.PosterUrl;
                movie.BackdropUrl = request.BackdropUrl != null ? request.BackdropUrl : movie.BackdropUrl;
                movie.OriginalLanguage = request.OriginalLanguage != null ? request.OriginalLanguage : movie.OriginalLanguage;
                movie.ReleaseDate = request.ReleaseDate != null ? request.ReleaseDate : movie.ReleaseDate;
                movie.RunTime = request.RunTime != null ? request.RunTime : movie.RunTime;
                movie.Price = request.Price != null ? request.Price : movie.Price;
                movie.UpdatedBy = userName;
                movie.UpdatedDate = DateTime.UtcNow;
                // save change
                var updateMovie = await _movieRepository.Add(movie);

                // update movie genres
                foreach (var genre in request.Genres)
                {
                    // check input genre validation
                    if (genre == null || _movieRepository.GetGenreById(genre.Id) == null) continue;

                    var movieGenre = await _movieRepository.GetMovieGenreByMovieIdAndGenreId(request.Id, genre.Id);
                    if (movieGenre == null)
                    {
                        var createdMovieGenre = await _movieRepository.AddMovieGenre(request.Id, genre.Id);
                        if (!createdMovieGenre)
                        {
                            return false;
                        }
                    }
                }
                return true;               
            }
            return false;
        }

        public async Task<List<MovieCardModel>> GetTopPurchasedMovie(DateTime fromDate, DateTime toDate)
        {
            var movies = await _purchaseRepository.GetTop30PurchasedMovies(fromDate, toDate);
            if (movies == null)
            {
                throw new Exception("No Purchase Record");
            }
            var movieCards = new List<MovieCardModel>();
            movieCards.AddRange(movies.Select(x => new MovieCardModel
            {
                Id = x.Id,
                Title = x.Title,
                PosterUrl = x.PosterUrl
            }));

            return movieCards;
        }

    }
}
