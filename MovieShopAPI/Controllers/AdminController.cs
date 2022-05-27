using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public AdminController(IAdminService adminService, IUserService userService)
        {
            _adminService = adminService;
            _userService = userService;
        }

        private int GetUserId()
        {
            return Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }


        [HttpPost]
        [Route("movie")]
        public async Task<IActionResult> CreateMovie(MovieCreateRequestModel request)
        {
            var userId = GetUserId();
            var userName = await _userService.GetUserNameById(userId);
            var isAuthorized = await _userService.IsAdmin(userId);
            if (!isAuthorized)
            {
                return Unauthorized(new { errorMessage = "Only Admin can access"});
            }

            var newMovie = await _adminService.CreateMovie(request, userName);
            if (newMovie == null || !newMovie)
            {
                return BadRequest(new { errorMessage = "Bad request"});
            }
            return Ok(newMovie);
        }

        [HttpPut]
        [Route("movie")]
        public async Task<IActionResult> EditMovie(MovieUpdateRequestModel request)
        {
            var userId = GetUserId();
            var userName = await _userService.GetUserNameById(userId);
            var isAuthorized = await _userService.IsAdmin(userId);
            if (!isAuthorized)
            {
                return Unauthorized(new { errorMessage = "Only Admin can access" });
            }

            var newMovie = await _adminService.UpdateMovie(request, userName);
            if (newMovie == null || !newMovie)
            {
                return BadRequest(new { errorMessage = "Bad request" });
            }

            return Ok(newMovie);
        }

        [HttpGet]
        [Route("top-purchased-movies")]
        public async Task<IActionResult> TopPurchasedMovies(DateTime fromDate, DateTime toDate)
        {
            var userId = GetUserId();
            var userName = await _userService.GetUserNameById(userId);
            var isAuthorized = await _userService.IsAdmin(userId);
            if (!isAuthorized)
            {
                return Unauthorized(new { errorMessage = "Only Admin can access" });
            }

            if (fromDate == null && toDate == null)
            {
                toDate = DateTime.Today;
                fromDate = DateTime.Today.AddDays(-90);
            } 
            else if (fromDate == null)
            {
                fromDate = toDate.AddDays(-90);
            }
            else if (toDate == null)
            {
                toDate = fromDate.AddDays(90);
            }

            var movies = await _adminService.GetTopPurchasedMovie(fromDate, toDate);
            if (movies == null || !movies.Any())
            {
                return NotFound(new { errorMessage = "No Movie Found"});
            }
            return Ok(movies);
        }
    }
}
