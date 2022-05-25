using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserRepository _userRepository;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                // 400 Bad Request
                return BadRequest();
            }
            var user = await _accountService.RegisterUser(model);

            return Ok(user);
        }

        [Route("check-email")]
        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var checkUser = await _userRepository.GetUserByEmail(email);
            if (checkUser == null)
            {
                return NotFound(new { ErrorMessage = "No Email Found" });
            }
            return Ok(checkUser);
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var user = await _accountService.LoginUser(model.Email, model.Password);
            if (user == null)
            {

                return BadRequest();
            }
            return Ok(user);
        }

        
    }
}
