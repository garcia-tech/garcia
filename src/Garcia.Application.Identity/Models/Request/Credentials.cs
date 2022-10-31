using System.ComponentModel.DataAnnotations;

namespace Garcia.Application.Identity.Models.Request
{
    public class Credentials
    {
        [Required(ErrorMessage = "Username cannot be empty")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }
    }
}
