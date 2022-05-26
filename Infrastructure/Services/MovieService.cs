using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<MovieDetailsModel> GetMovieDetails(int movieId)
        {
            var movie = await _movieRepository.GetById(movieId);
            if (movie == null)
            {
                return null;
            }

            var movieDetails = new MovieDetailsModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Overview = movie.Overview,
                Tagline = movie.Tagline,
                Budget = movie.Budget,
                Revenue = movie.Revenue,
                ImdbUrl = movie.ImdbUrl,
                TmdbUrl = movie.TmdbUrl,
                PosterUrl = movie.PosterUrl,
                BackdropUrl = movie.BackdropUrl,
                OriginalLanguage = movie.OriginalLanguage,
                ReleaseDate = movie.ReleaseDate.GetValueOrDefault(),
                RunTime = movie.RunTime,
                Price = movie.Price                
            };


            foreach (var trailer in movie.Trailers)
            {
                movieDetails.Trailers.Add(new TrailerModel { Id = trailer.Id, Name = trailer.Name, TrailerUrl = trailer.TrailerUrl });
            }

            foreach (var genre in movie.MoviesOfGenre)
            {
                movieDetails.Genres.Add(new GenreModel { Id = genre.GenreId, Name = genre.Genre.Name });
            }

            foreach (var cast in movie.MoviesOfCast)
            {
                movieDetails.Casts.Add(new CastModel { Id = cast.CastId, Name = cast.Cast.Name, Character = cast.Character, ProfilePath = cast.Cast.ProfilePath });
            }

            movieDetails.RatingAvg = Decimal.Round(movie.Reviews.Average(r => r.Rating), 1) ;

            return movieDetails;
        }

        public async Task<List<MovieCardModel>> GetTop30GrossingMovies()
        {
            //var movieRepo = new MovieRepository();
            //var movies = movieRepo.GetTop30GrossingMovies();
            var movies = await _movieRepository.GetTop30GrossingMovies();

            var movieCards = new List<MovieCardModel>();
            foreach (var movie in movies)
            {
                movieCards.Add(new MovieCardModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    PosterUrl = movie.PosterUrl
                });
            }

            return movieCards;
        }

        public async Task<PagedResultSet<MovieCardModel>> GetMoviesByGenrePagination(int genreId, int pageSize = 30, int pageNumber = 1)
        {
            var pagedMovies = await _movieRepository.GetMoviesByGenres(genreId, pageSize, pageNumber);

            var movieCards = new List<MovieCardModel>();
            movieCards.AddRange(pagedMovies.Data.Select(m => new MovieCardModel
            {
                Id = m.Id,
                PosterUrl = m.PosterUrl,
                Title = m.Title
            }));

            return new PagedResultSet<MovieCardModel>(movieCards, pageNumber, pageSize, pagedMovies.Count);
        }

        public async Task<List<MovieCardModel>> GetTop30RatedMovies()
        {
            var movies = await _movieRepository.GetTop30RatedMovies();
            var movieCards = new List<MovieCardModel>();
            foreach (var movie in movies)
            {
                movieCards.Add(new MovieCardModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    PosterUrl = movie.PosterUrl
                });
            }

            return movieCards;

        }

        public async Task<PagedResultSet<ReviewDetailsModel>> GetReviewsByMoviePagination(int id, int pageSize = 30, int pageNumber = 1)
        {
            var pagedReviews = await _movieRepository.GetReviews(id, pageSize, pageNumber);

            var review = new List<ReviewDetailsModel>();
            review.AddRange(pagedReviews.Data.Select(r => new ReviewDetailsModel
            {
                MovieId = r.MovieId,
                UserId = r.UserId,
                Rating = r.Rating,
                ReviewText = r.ReviewText,
                Movie = new MovieCardModel
                {
                    Id = r.MovieId,
                    Title = r.Movie.Title,
                    PosterUrl = r.Movie.PosterUrl,
                }
            }));

            return new PagedResultSet<ReviewDetailsModel>(review, pageNumber, pageSize, pagedReviews.Count);
        }

        public async Task<PagedResultSet<MovieCardModel>> GetMoviesByPagination(int pageSize = 30, int pageNumber = 1)
        {
            var pagedMovies = await _movieRepository.GetMovies(pageSize, pageNumber);

            var movies = new List<MovieCardModel>();
            movies.AddRange(pagedMovies.Data.Select(m => new MovieCardModel
            {
                Id = m.Id,
                Title = m.Title,
                PosterUrl = m.PosterUrl
            }));

            return new PagedResultSet<MovieCardModel>(movies, pageNumber, pageSize, pagedMovies.Count);
        }
    }
}
