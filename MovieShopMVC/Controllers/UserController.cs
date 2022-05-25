using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MovieShopMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        private int GetUserId()
        {
            return Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(PurchaseRequestModel request)
        {
            var uerId = GetUserId();
            var purchase = await _userService.PurchaseMovie(request, uerId);
            if (purchase == null)
            {
                throw new Exception("Purchase failed");
            }

            return RedirectToAction("Purchases");
        }

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
            var userId = GetUserId();
            var purchases = await _userService.GetAllPurchasesForUser(userId);
            // call UserService -> GetMoviesPurchasedByUser(int userId) -> List<MovieCard>
            return View(purchases);
        }

        [HttpGet]
        public async Task<IActionResult> Favorite()
        {
            var uerId = Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var uerId = Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Reviews()
        {
            var uerId = GetUserId();
            var reviews = await _userService.GetAllReviewsByUser(uerId);
            return View(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewRequestModel model)
        {
            var uerId = GetUserId();
            var reviews = await _userService.GetAllReviewsByUser(uerId);
            return View(reviews);
        }


        //[HttpPost]
        //public async Task<IActionResult> AddReview(int movieId, decimal rating = 9, string text = " ")
        //{
        //    var uerId = GetUserId();
        //    var result = await _userService.AddMovieReview(new ReviewRequestModel
        //    {
        //        MovieId = movieId,
        //        Rating = rating,
        //        ReviewText = text,
        //        UserId = uerId
        //    });
        //    if (result)
        //    {
        //        return LocalRedirect("~/");
        //    }
        //    else
        //    {
        //        throw new Exception("add review failed");
        //    }
        //}
    }
}
