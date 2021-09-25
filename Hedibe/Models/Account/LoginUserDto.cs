using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Models.Account
{
    public class LoginUserDto
    {
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
