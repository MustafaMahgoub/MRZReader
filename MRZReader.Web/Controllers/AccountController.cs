using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MRZReader.Web.ViewModels;

namespace MRZReader.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(
            ILogger<AccountController> logger, 
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> singInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = singInManager;
        }
        
        #region Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                return View("Error", e.Message.ToString());
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new IdentityUser()
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };
                    var res = await _userManager.CreateAsync(user, model.Password);
                    if (res.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        Log($"User {model.Email} registered successfully.");
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                return View("Error", e.Message.ToString());
            }
        }
        #endregion

        #region Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                return View("Error", e.Message.ToString());
            }
        }
        #endregion

        #region Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                return View("Error", e.Message.ToString());
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _signInManager.PasswordSignInAsync(
                        model.Email,
                        model.Password,
                        model.RememberMe,
                        false);

                    if (res.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Invalid Login");
                }
                return View();
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                return View("Error", e.Message.ToString());
            }
        }
        #endregion

        private void Log(string msg, bool isEception = false)
        {
            if (isEception)
            {
                _logger.LogError($"[MRZ_Logs] {msg}.");
            }
            else
            {
                _logger.LogTrace($"[MRZ_Logs] {msg}.");
            }
        }
    }
}