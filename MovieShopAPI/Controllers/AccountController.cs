using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserRepository _userRepository;

        public AccountController(IAccountService accountService, IUserRepository userRepository)
        {
            _accountService = accountService;
            _userRepository = userRepository;
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

                return Unauthorized(new { ErrorMessage = "Wrong Email/Password" });
            }

            // return a token
            var jwtToken = GenerateJwtToken(user);

            return Ok(new { token = jwtToken });

            //return Ok(user);
        }

        private string GenerateJwtToken(UserLoginResponseModel user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Language", "English")
            };

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyTopSecretKeyForJWTTokenGenerationVersion1sfhbksjbfkjsdfjkdsnb"));

            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenExpiration = DateTime.UtcNow.AddHours(2);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDetails = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Expires = tokenExpiration,
                SigningCredentials = credentials,
                Issuer = "MovieShop, Inc",
                Audience = "MovieShop Users"
            };

            var encodedJwt = tokenHandler.CreateToken(tokenDetails);

            return tokenHandler.WriteToken(encodedJwt);
        }

        
    }
}
