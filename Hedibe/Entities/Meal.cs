﻿using System;
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
        public string CookingDescription { get; set; }
        public virtual List<Product> Products { get; set; }
        public bool Verified { get; set; }
        public int? OwnerId { get; set; }
        public virtual User Owner { get; set; }


        public decimal CalculateTotalCalories()
        {
            double? TotalCalories = 0;
            foreach (var product in this.Products)
            {
                TotalCalories += product.Calories;
            }
            return Math.Round((decimal)TotalCalories, 2);
        }
        public decimal CalculateTotalFat()
        {
            double? TotalFat = 0;
            foreach (var product in this.Products)
            {
                TotalFat += product.TotalFat;
            }
            return Math.Round((decimal)TotalFat, 2);
        }
        public decimal CalculateTotalProtein()
        {
            double? TotalProtein = 0;
            foreach (var product in this.Products)
            {
                TotalProtein += product.Protein;
            }
            return Math.Round((decimal)TotalProtein, 2);
        }
        public decimal CalculateTotalCarbohydrate()
        {
            double? TotalCarbohydrate = 0;
            foreach (var product in this.Products)
            {
                TotalCarbohydrate += product.Carbohydrate;
            }
            
            return Math.Round((decimal)TotalCarbohydrate, 2);
        }

        public string CookingTimeInHourAndMinutes()
        {
            int hour = (int)this.CookingTime/60;
            int minutes = (int)this.CookingTime % 60;
            

            return $"{hour}:{minutes}";
        }
    }
}
