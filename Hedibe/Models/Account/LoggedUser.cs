using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Models.Account
{
    public class LoggedUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
