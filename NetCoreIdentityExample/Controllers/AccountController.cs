using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentityExample.Models.Entities;
using NetCoreIdentityExample.Models.IdentityViewModels;

namespace NetCoreIdentityExample.Controllers
{
    public class AccountController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var userModel = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(userModel, model.Password);

            if (result.Succeeded)
            {
                var response = await _userManager.GenerateEmailConfirmationTokenAsync(userModel);
            }
            else
            {
                return View(result.Errors);
            }

            return RedirectToAction("Index", "Home");
        }
    }



}
