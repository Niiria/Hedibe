using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hedibe.Entities;

namespace Hedibe.Models.ShoppingLists
{
    public class ShoppingListAddDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual List<Product> CurrentProducts { get; set; }
        public int? OwnerId { get; set; }
        public virtual User Owner { get; set; }
    }
}
