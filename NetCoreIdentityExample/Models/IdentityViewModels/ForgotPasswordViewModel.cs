using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentityExample
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Code { get; set; }
    }
}