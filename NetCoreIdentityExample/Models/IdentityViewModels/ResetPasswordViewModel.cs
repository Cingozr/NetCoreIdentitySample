using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreIdentityExample.Models.IdentityViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(otherProperty: "Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmedPassword { get; set; }
    }
}
