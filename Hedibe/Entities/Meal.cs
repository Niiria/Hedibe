using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Entities
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CookingTime { get; set; }
        public int? Difficulty { get; set; }
        public int? TotalCalories { get; set; }
        public string CookingDescription { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public bool Verified { get; set; }
        public int? OwnerId { get; set; }
        public virtual User Owner { get; set; }
    }
}
