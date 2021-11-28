using Hedibe.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hedibe.Models.Meals
{
    public class MealAddDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Required*")]
        [MinLength(3)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required*")]
        [MinLength(3)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required*")]
        public int? CookingTime { get; set; }

        [Required(ErrorMessage = "Required*")]
        [Range(1, 5)]
        public int? Difficulty { get; set; }
        public double? TotalCalories { get; set; }

        [Required(ErrorMessage = "Required*")]
        [MinLength(3)]
        public string CookingDescription { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual List<Product> CurrentProducts { get; set; }
        public bool Verified { get; set; }
        public int? OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public decimal CalculateTotalCalories()
        {
            double? TotalCalories = 0;
            foreach (var product in this.CurrentProducts)
            {
                TotalCalories += product.Calories;
            }
            return Math.Round((decimal)TotalCalories, 2);
        }
        public decimal CalculateTotalFat()
        {
            double? TotalFat = 0;
            foreach (var product in this.CurrentProducts)
            {
                TotalFat += product.TotalFat;
            }
            return Math.Round((decimal)TotalFat, 2);
        }
        public decimal CalculateTotalProtein()
        {
            double? TotalProtein = 0;
            foreach (var product in this.CurrentProducts)
            {
                TotalProtein += product.Protein;
            }
            return Math.Round((decimal)TotalProtein, 2);
        }
        public decimal CalculateTotalCarbohydrate()
        {
            double? TotalCarbohydrate = 0;
            foreach (var product in this.CurrentProducts)
            {
                TotalCarbohydrate += product.Carbohydrate;
            }

            return Math.Round((decimal)TotalCarbohydrate, 2);
        }

        public string CookingTimeInHourAndMinutes()
        {
            int hour = (int)this.CookingTime / 60;
            int minutes = (int)this.CookingTime % 60;


            return $"{hour}:{minutes}";
        }
    }
}

