using ApplicationCore.Contracts.Services;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;

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
            try 
            {
                var user = await _accountService.RegisterUser(model);
            }
            catch (ConflictException ex)
            {
                throw;
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
                var user = _accountService.LoginUser(model.Email, model.Password);
                // create cookie : userid, email -> encrypted, expiration time
                if (user != null)
                {
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
    }
}
