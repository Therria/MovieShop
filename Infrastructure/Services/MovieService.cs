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

        public MovieDetailsModel GetMovieDetails(int movieId)
        {
            var movie = _movieRepository.GetById(movieId);
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

            return movieDetails;
        }

        public List<MovieCardModel> GetTop30GrossingMovies()
        {
            //var movieRepo = new MovieRepository();
            //var movies = movieRepo.GetTop30GrossingMovies();
            var movies = _movieRepository.GetTop30GrossingMovies();

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
    }
}
