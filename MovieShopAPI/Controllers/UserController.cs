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
    public class UserController : ControllerBase
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

        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> Details()
        {
            var userId = GetUserId();
            var userDetails = await _userService.GetUserDetails(userId);
            if (userDetails == null)
            {
                return NotFound(new { errorMessage = "No User Found" });
            }
            return Ok(userDetails);
        }

        [HttpPost]
        [Route("purchase-movie")]
        public async Task<IActionResult> Buy(PurchaseRequestModel request)
        {
            var userId = GetUserId();
            var purchase = await _userService.PurchaseMovie(request, userId);
            if (purchase == null)
            {
                return BadRequest(new { errorMessage = "Bad Request" });
            }
            return Ok(purchase);
        }

        [HttpGet]
        [Route("purchases")]
        public async Task<IActionResult> GetMoviesPurchasedByUser()
        {
            var userId = GetUserId();
            var purchases = await _userService.GetAllPurchasesForUser(userId);
            if (purchases == null)
            {
                return NotFound(new { errorMessage = "No Purchase Found" });
            }

            return Ok(purchases);
        }

        [HttpGet]
        [Route("purchase-details/{movieId}")]
        public async Task<IActionResult> PurchaseDetail(int movieId)
        {
            var userId = GetUserId();
            var purchase = await _userService.GetPurchasesDetails(userId, movieId);
            if (purchase == null)
            {
                return NotFound(new { errorMessage = "No Purchase Found" });
            }

            return Ok(purchase);
        }

        [HttpGet]
        [Route("check-movie-purchased/{movieId}")]
        public async Task<IActionResult> IsPurchased(int movieId)
        {
            var userId = GetUserId();
            var purchase = await _userService.IsMoviePurchased(userId, movieId);
            if (purchase == null)
            {
                return NotFound(new { errorMessage = "No Purchase Found" });
            }

            return Ok(purchase);
        }

        [HttpPost]
        [Route("add-review")]
        public async Task<IActionResult> AddReview(ReviewRequestModel model)
        {
            var userId = GetUserId();
            model.UserId = userId;
            //ReviewRequestModel model = new ReviewRequestModel()
            //{
            //    UserId = userId,
            //    MovieId = MovieId,
            //    Rating = Rating,
            //    ReviewText = reviewText
            //};

            var addReview = await _userService.AddMovieReview(model);
            if (addReview == null || !addReview)
            {
                return BadRequest(new { errorMessage = "Bad Request" });
            }
            return Ok(addReview);

        }

        [HttpPut]
        [Route("edit-review")]
        public async Task<IActionResult> EditReview(ReviewRequestModel model)
        {
            var userId = GetUserId();
            model.UserId = userId;
            //ReviewRequestModel model = new ReviewRequestModel()
            //{
            //    UserId = userId,
            //    MovieId = MovieId,
            //    Rating = Rating,
            //    ReviewText = reviewText
            //};
            var editReview = await _userService.UpdateMovieReview(model);
            if (editReview == null || !editReview)
            {
                return BadRequest(new { errorMessage = "Bad Request" });
            }
            return Ok(editReview);
        }

        [HttpDelete]
        [Route("delete-review/{movieId}")]
        public async Task<IActionResult> DeleteReview(int movieId)
        {
            var userId = GetUserId();
            var deleteReview = await _userService.DeleteMovieReview(userId, movieId);
            if (deleteReview == null || !deleteReview)
            {
                return BadRequest(new { errorMessage = "Bad Request" });
            }
            return Ok(deleteReview);
        }

        [HttpGet]
        [Route("movie-reviews")]
        public async Task<IActionResult> Reviews()
        {
            var userId = GetUserId();
            var reviews = await _userService.GetAllReviewsForUser(userId);
            if (reviews == null)
            {
                return NotFound(new { errorMessage = "No Review Found" });
            }

            return Ok(reviews);
        }


        [HttpPost]
        [Route("favorite")]
        public async Task<IActionResult> Favorite(FavoriteRequestModal favoriteRequest)
        {
            var userId = GetUserId();
            favoriteRequest.UserId = userId;
            //var favoriteRequest = new FavoriteRequestModal
            //{
            //    UserId = userId,
            //    MovieId = MovieId
            //};
            var addFavorite = await _userService.AddUserFavorite(favoriteRequest);
            if (addFavorite == null || !addFavorite)
            {
                return BadRequest(new { errorMessage = "Bad Request" });
            }
            return Ok(addFavorite);
        }

        [HttpPost]
        [Route("un-favorite")]
        public async Task<IActionResult> UnFavorite(FavoriteRequestModal favoriteRequest)
        {
            var userId = GetUserId();
            favoriteRequest.UserId = userId;
            //var favoriteRequest = new FavoriteRequestModal
            //{
            //    UserId = userId,
            //    MovieId = MovieId
            //};
            var deleteFavorite = await _userService.DeleteUserFavorite(favoriteRequest);
            if (deleteFavorite == null || !deleteFavorite)
            {
                return BadRequest(new { errorMessage = "Bad Request" });
            }
            return Ok(deleteFavorite);
        }

        [HttpGet]
        [Route("check-movie-favorite/{movieId}")]
        public async Task<IActionResult> CheckIsFavorite(int MovieId)
        {
            var userId = GetUserId();
            var checkFavorite = await _userService.IsMovieFavorited(userId, MovieId);
            if (checkFavorite == null)
            {
                return NotFound(new { errorMessage = "No Favorite Status Found" });
            }
            return Ok(checkFavorite);
        }

        [HttpGet]
        [Route("favorites")]
        public async Task<IActionResult> Favorites()
        {
            var userId = GetUserId();
            var favorites = await _userService.GetAllFavoritesForUser(userId);
            if (favorites == null)
            {
                return NotFound(new { errorMessage = "No Favorite Found" });
            }
            return Ok(favorites);
        }




}
}
