using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hedibe.Entities;

namespace Hedibe.Models.ShoppingLists
{
    public class ShoppingListAddDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public int? OwnerId { get; set; }
        public virtual User Owner { get; set; }
    }
}
