using Microsoft.OpenApi.MicrosoftExtensions;
using System.ComponentModel.DataAnnotations;

namespace WebAPI5.DataModel
{
    public class RegisterUser
    {
        [Required(ErrorMessage="User Name is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        public string Password { get; set; }
    }
}
