using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
