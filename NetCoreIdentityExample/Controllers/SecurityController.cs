using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentityExample.Helpers.MailHelper;
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
        MailHelper mailHelper = new MailHelper();

        public SecurityController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        #region Login Islemi
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var userModel = await _userManager.FindByNameAsync(loginViewModel.UserName);
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


            return View(loginViewModel);


        }
        #endregion

        #region Register Islemi
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
                var confirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(userModel);
                var callBackUrl = $"{HttpContext.Request.Host}/{Url.Action("ConfirmationEmail", "Security", new { userId = userModel.Id, code = confirmationCode })}";

                mailHelper.MailSend("recaicingz@gmail.com", "recaicingz@gmail.com", "Email Onay Mesaji", callBackUrl);

                return RedirectToAction("Index", "Account");
            }

            return View(registerViewModel);

        }
        #endregion

        #region ForgotPassword Islemi
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
            var callBackUrl = $"https://{HttpContext.Request.Host}{Url.Action("ResetPassword", "Security", new { userEmail = userModel.Email, code = confirmationCode })}"; //TODO : Reset Password islem sayfasi

            mailHelper.MailSend("recaicingz@gmail.com", "recaicingz@gmail.com", "Yeni Parola", callBackUrl);

            return RedirectToAction("ForgotPasswordEmailSend", "Security"); //TODO : ForgotPasswordEmailSend bilgi sayfasi
        }
        #endregion

        #region Email Onaylama
        public async Task<IActionResult> ConfirmationEmail(string userId, string code)
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
                return RedirectToAction("EmailConfirm", "Security"); //TODO : Confirmed bilgi sayfasi

            return RedirectToAction("Index", "Account");
        }
        #endregion

        #region Forgot Password Sonrari Reset Password Islemi
        public IActionResult ResetPassword(string userEmail, string code)
        {
            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(code))
            {
                throw new ApplicationException(message: "Email ya da parola yenileme kodu hatali");
            }

            var model = new ResetPasswordViewModel { Code = code, Email = userEmail };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordViewModel);


            var userModel = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (userModel == null)
                throw new ApplicationException("Kullanici bulunamadi");

            var result = await _userManager.ResetPasswordAsync(userModel, resetPasswordViewModel.Code, resetPasswordViewModel.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirm", "Security");

            return View();
        }
        #endregion

        #region Bilgi Sayfalari
        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }

        public IActionResult EmailConfirm()
        {
            return View();
        }

        public IActionResult ForgotPasswordEmailSend()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion



        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Account");
        }



    }
}