using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Contracts.Services;

namespace MovieShopAPI.Controllers
{
    //Attribute based routing
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieServive)
        {
            _movieService = movieServive;
        }

        //[Route("")]
        [HttpGet]
        public async Task<IActionResult> GetByPagination()
        {
            var movieCards = await _movieService.GetTop30GrossingMovies();
            if (movieCards == null)
            {
                return NotFound();
            }
            return Ok(movieCards);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieService.GetMovieDetails(id);
            if (movie == null)
            {
                return NotFound(new { ErrorMessage = "No Movie Found" });
            }
            return Ok(movie);
        }

        [Route("top-rated")]
        [HttpGet]
        public async Task<IActionResult> TopRated(int id)
        {
            var movies = await _movieService.GetTop30RatedMovies();
            if (movies == null || !movies.Any())
            {
                return NotFound(new { errorMessage = "No Movie Found" });
            }
            return Ok(movies);
        }

        // api/movies/top-grossing
        [Route("top-grossing")]
        [HttpGet]
        public async Task<IActionResult> TopGrossing()
        {
            var movies = await _movieService.GetTop30GrossingMovies();

            // return JSON data and always return proper HTTP status code
            if (movies == null || !movies.Any())
            {
                // 404 NotFound
                return NotFound(new { errorMessage = "No Movies Found"});
            }

            // ASP.NET Core API will automatically serialize C# objects into JSON Objects
            // System.Text.Json <= using this one
            // Newtonsoft.Json (<= old version)

            // 200 OK
            return Ok(movies);
        }

        [Route("genre/{genreId}")]
        [HttpGet]
        public async Task<IActionResult> GetByGenre(int genreId)
        {
            var movies = await _movieService.GetMoviesByGenrePagination(genreId, 30);
            if (movies == null)
            {
                return NotFound(new { errorMessage = "No Genres Found" });
            }
            return Ok(movies);
        }

        [Route("{id}/reviews")]
        [HttpGet]
        public async Task<IActionResult> GetReviews(int id)
        {
            return Ok();
        }

        
    }
}
