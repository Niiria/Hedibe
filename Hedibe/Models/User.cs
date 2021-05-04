using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Models
{
    public class User
    {
        [Key, Required]
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Username { get; set; }
        [Required, MaxLength(512)]
        public string Password { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
