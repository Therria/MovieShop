using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            return View();
        }
    }
}
