using ApplicationCore.Contracts.Services;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MovieShopMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // showing the empty page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // when user clicks on Submit/Register button
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            // check if validated
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            var user = await _accountService.RegisterUser(model);

            if (user == null)
            {
                throw new Exception("Register failed");
            }            
            
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            try
            {
                var user = await _accountService.LoginUser(model.Email, model.Password);
                if (user != null)
                {
                    // user/password are matching, then go creating cookie on client side (browser) --> cookie based authentication

                    // Claim called Admin Role to enter admin page
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.GivenName, user.FirstName),
                        new Claim(ClaimTypes.Surname, user.LastName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        // new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth..ToShortGateString()),
                        new Claim("language", "English")
                    };

                    // Identity
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // create cookie with above detail
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // expiration time & cookie name --> set in program.cs

                    // redirect to home page
                    return LocalRedirect("~/");
                }                
            }
            catch (Exception ex)
            {
                return View();
                throw;
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
