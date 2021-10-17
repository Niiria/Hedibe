using Hedibe.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Models.ShoppingLists
{
    public class ProductsList
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Product> CurrentProducts { get; set; }
    }
}
