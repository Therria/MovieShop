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

        private int GetUserId()
        {
            return Convert.ToInt32(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        [HttpPost]
        [Route("movie")]
        public async Task<IActionResult> CreateMovie()
        {
            return Ok();
        }

        [HttpPut]
        [Route("movie")]
        public async Task<IActionResult> EditMovie()
        {
            return Ok();
        }

        [HttpGet]
        [Route("top-purchased-movies")]
        public async Task<IActionResult> TopPurchasedMovies()
        {
            return Ok();
        }
    }
}
