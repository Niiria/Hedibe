using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Models.Products
{
    public class ProductAddDto
    {
        [Required(ErrorMessage = "Required*")]
        [MinLength(3)]
        public string Name { get; set; }
        public int? AmountPer { get; set; } = 100;
        [Required(ErrorMessage = "Required*")]
        [Range(0, int.MaxValue, ErrorMessage = "Calories can not be negative ")]
        public double? Calories { get; set; }
        public double? TotalFat { get; set; }
        public double? Protein { get; set; }
        public double? Carbohydrate { get; set; }
        public bool Verified { get; set; }
        public int? OwnerId { get; set; }
    }
}
