using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? AmountPer { get; set; } = 100;
        public double? Calories { get; set; }
        public double? TotalFat { get; set; }
        public double? Protein { get; set; }
        public double? Carbohydrate { get; set; }
        public bool Verified { get; set; }
        public int? OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public List<Meal> Meals { get; set; }
        public List<ShoppingList> ShoppingLists { get; set; }
    }
}
