using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Entities
{
    public class ShoppingList
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public List<Product> Products { get; set; }
    }
}
