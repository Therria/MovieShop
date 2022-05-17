using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public IActionResult Details(int id)
        {
            var movie = _movieService.GetMovieDetails(id);
            return View(movie);
        }
    }
}
