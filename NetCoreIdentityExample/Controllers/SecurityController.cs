using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentityExample.Models.Entities;
using NetCoreIdentityExample.Models.IdentityViewModels;
using System;
using System.Threading.Tasks;

namespace NetCoreIdentityExample.Controllers
{
    public class SecurityController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public SecurityController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }




        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var userModel = await _userManager.FindByEmailAsync(loginViewModel.UserName);
            if (userModel != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(userModel))
                {
                    ModelState.AddModelError("", "Email your confirmed please");
                    return View(loginViewModel);
                }
            }

            var loginResult = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);
            if (loginResult.Succeeded)
                return RedirectToAction("Index", "Account");

            ModelState.AddModelError("", "Bir hata olustu");
            return View(loginViewModel);


        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
                return View(registerViewModel);


            var userModel = new ApplicationUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email
            };
            var registerResult = await _userManager.CreateAsync(userModel, registerViewModel.Password);

            if (registerResult.Succeeded)
            {
                var confirmationCode = _userManager.GenerateEmailConfirmationTokenAsync(userModel);
                var callBackUrl = Url.Action("ConfirmationUrl", "Security", new { userId = userModel.Id, code = confirmationCode });

                //Send Email

                return RedirectToAction("Index", "Account");
            }

            return View(registerViewModel);

        }


        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordModel)
        {
            if (string.IsNullOrEmpty(forgotPasswordModel.Email))
                return View();


            var userModel = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (userModel == null)
                return View();

            var confirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(userModel);
            var callBackUrl = Url.Action("ResetPassword", "Security", new { userId = userModel.Id, code = confirmationCode });

            //Email send callback url  

            return RedirectToAction("ForgotPasswordEmailSend", "Security");
        }

        public IActionResult ForgotPasswordEmailSend()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Account");
        }

        public async Task<IActionResult> ConfirmationUrl(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                return RedirectToAction("Index", "Account");
            }

            var userModel = await _userManager.FindByIdAsync(userId);
            if (userModel == null)
                throw new ApplicationException("Boyle bir kullanici bulunamadi");

            var result = await _userManager.ConfirmEmailAsync(userModel, code);
            if (result.Succeeded)
                return RedirectToAction("Confirmed", "Account");

            return RedirectToAction("Index", "Account");
        }

















        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}