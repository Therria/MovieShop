using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MovieShopMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Purchases()
        {
            // check whether loged in
            // if not, redirect to login page
            // if yes, [decrypt cookie -> get id] use id to direct to db to get purchase info
            // var data = this.HttpContext.Request.Cookies["MovieShopAuthCookie"];

            // Using ASP.NET Core Middleware (done in program.cs)
            //var isLogedIn = this.HttpContext.User.Identity.IsAuthenticated;
            //if (!isLogedIn)
            //{
            //    // redirect to login page
            //}
            //var userId = this.HttpContext.User.Claims.FirstOrDefault(x => x.ValueType == ClaimType.Identifier)?.Value;

            // Instead, using Filters in ASP.NET
            var uerId = Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            // call UserService -> GetMoviesPurchasedByUser(int userId) -> List<MovieCard>
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            var uerId = Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Reviews()
        {
            var uerId = Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View();
        }
    }
}
