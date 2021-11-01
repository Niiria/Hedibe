using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hedibe.Entities;

namespace Hedibe.Models.ShoppingLists
{
    public class ShoppingListAddDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Required*")]
        [MinLength(3)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required*")]
        [MinLength(3)]
        public string Description { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual List<Product> CurrentProducts { get; set; }
        public int? OwnerId { get; set; }
        public virtual User Owner { get; set; }
    }
}
