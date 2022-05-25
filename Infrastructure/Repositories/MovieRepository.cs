using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Models;

namespace Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Movie>> GetTop30GrossingMovies()
        {

            // var top30Movies = movies.GetMovies().OrderByDescending(m => m.Revenue).Take(30);
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToListAsync();

            return movies;
        }

        public async Task<List<Movie>> GetTop30RatedMovies()
        {
            var topRatedIds = await _dbContext.Reviews.Include(r => r.Movie).GroupBy(r => r.MovieId)
                .Select(m => new
                {
                    MovieId = m.Key,
                    RatingAve = m.Average(r => r.Rating)
                })
                .OrderByDescending(m => m.RatingAve)
                .Select(r => r.MovieId)
                .Take(30).ToListAsync();
            var movies = await _dbContext.Movies.Where(m => topRatedIds.Contains(m.Id)).ToListAsync();

            return movies;
        }

        public override async Task<Movie> GetById(int id)
        {
            // SELECT ... FROM joint table below WHERE m.id = id
            // Movie LEFT JOIN (MovieGenre JOIN Genre) LEFT JOIN (MovieCast JOIN Cast) LEFT JOIN (Trailer)
            // SQL Code:
            // SELECT[t].[Id], [t].[BackdropUrl], [t].[Budget], [t].[CreatedBy], [t].[CreatedDate], [t].[ImdbUrl], [t].[OriginalLanguage], [t].[Overview], [t].[PosterUrl], [t].[Price], [t].[ReleaseDate], [t].[Revenue], [t].[RunTime], [t].[Tagline], [t].[Title], [t].[TmdbUrl], [t].[UpdatedBy], [t].[UpdatedDate], [t0].[MovieId], [t0].[GenreId], [t0].[Id], [t0].[Name], [t1].[MovieId], [t1].[CastId], [t1].[Character], [t1].[Id], [t1].[Gender], [t1].[Name], [t1].[ProfilePath], [t1].[TmdbUrl], [t2].[Id], [t2].[MovieId], [t2].[Name], [t2].[TrailerUrl]
            // FROM(
            //    SELECT TOP(1)[m].[Id], [m].[BackdropUrl], [m].[Budget], [m].[CreatedBy], [m].[CreatedDate], [m].[ImdbUrl], [m].[OriginalLanguage], [m].[Overview], [m].[PosterUrl], [m].[Price], [m].[ReleaseDate], [m].[Revenue], [m].[RunTime], [m].[Tagline], [m].[Title], [m].[TmdbUrl], [m].[UpdatedBy], [m].[UpdatedDate]
            //    FROM[Movie] AS[m]
            //    WHERE[m].[Id] = @__id_0
            // ) AS[t]
            // LEFT JOIN(
            //    SELECT[m0].[MovieId], [m0].[GenreId], [g].[Id], [g].[Name]
            //    FROM [MovieGenre] AS [m0]
            //    INNER JOIN [Genre] AS[g] ON [m0].[GenreId] = [g].[Id]
            // ) AS[t0] ON[t].[Id] = [t0].[MovieId]
            // LEFT JOIN(
            //    SELECT[m1].[MovieId], [m1].[CastId], [m1].[Character], [c].[Id], [c].[Gender], [c].[Name], [c].[ProfilePath], [c].[TmdbUrl]
            //    FROM [MovieCast] AS [m1]
            //    INNER JOIN [Cast] AS[c] ON [m1].[CastId] = [c].[Id]
            // ) AS[t1] ON[t].[Id] = [t1].[MovieId]
            // LEFT JOIN[Trailer] AS[t2] ON[t].[Id] = [t2].[MovieId]
            // ORDER BY[t].[Id], [t0].[MovieId], [t0].[GenreId], [t0].[Id], [t1].[MovieId], [t1].[CastId], [t1].[Id]
            var movie = await _dbContext.Movies.Include(m => m.MoviesOfGenre).ThenInclude(m => m.Genre)
                .Include(m => m.MoviesOfCast).ThenInclude(m => m.Cast)
                .Include(m => m.Trailers)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movie;
        }

        public async Task<PagedResultSet<Movie>> GetMoviesByGenres(int genreId, int pageSize = 30, int pageNumber = 1)
        {
            var totalMoviesCountByGenre = await _dbContext.MovieGenres.Where(m => m.GenreId == genreId).CountAsync();

            if (totalMoviesCountByGenre == 0)
            {
                throw new Exception("No Movies Found for that genre");
            }

            var movies = await _dbContext.MovieGenres.Where(g => g.GenreId == genreId).Include(m => m.Movie)
                .OrderBy(m => m.MovieId)
                // using SELECT to convert one entity object to another one
                .Select(m => new Movie
                {
                    Id = m.MovieId,
                    PosterUrl = m.Movie.PosterUrl,
                    Title = m.Movie.Title
                })
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies, pageNumber, pageSize, totalMoviesCountByGenre);

            return pagedMovies;
        }

        public async Task<List<Genre>> GetGenreList()
        {
            var genres = await _dbContext.Genres.OrderBy(g => g.Name).ToListAsync();
            return genres;
            
        }

        
    }
}
