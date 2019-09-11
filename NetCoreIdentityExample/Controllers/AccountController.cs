using Microsoft.AspNetCore.Mvc;
using NetCoreIdentityExample.Models.Entities;
using NetCoreIdentityExample.Models.IdentityViewModels;
using System.Threading.Tasks;

namespace NetCoreIdentityExample.Controllers
{
    public class AccountController : BaseController
    {
        
        public IActionResult Index()
        {
            return View();
        }


        
    }



}
