using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreIdentityExample.Models.Entities
{
    public class ApplicationUser : IdentityUser<int> //int verilmesi zorunlu degildir. Bu Primary key olacak(Unique) propertynin tipini belirtiyoruz.
    {

    }
}
