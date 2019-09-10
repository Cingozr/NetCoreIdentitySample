using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreIdentityExample.Controllers
{
    public class SecurityController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}