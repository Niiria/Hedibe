using Hedibe.Data;
using Hedibe.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe
{
    
    public class HedibeSeeder
    {
        private readonly HedibeDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        public HedibeSeeder(HedibeDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Users.Any())
                {
                    var users = GetUsers();
                    _dbContext.Users.AddRange(users);
                    _dbContext.SaveChanges();
                }
               
                if (!_dbContext.Products.Any())
                {
                    var products = GetProducts();
                    _dbContext.Products.AddRange(products);
                    _dbContext.SaveChanges();
                }

            }
        }

        private IEnumerable<User> GetUsers()
        {
            User admin = new() { Username = "admin", Email="admin@hedibe.com", RoleId=1 };
            var adminHash = _passwordHasher.HashPassword(admin, "admin");
            admin.PasswordHash = adminHash;

            User mod = new() { Username = "mod", Email = "mod@hedibe.com", RoleId = 2 };
            var modHash = _passwordHasher.HashPassword(mod, "mod");
            mod.PasswordHash = modHash;

            User user = new() { Username = "user", Email = "user@hedibe.com", RoleId = 3 };
            var userHash = _passwordHasher.HashPassword(user, "user");
            user.PasswordHash = userHash;

            var users = new List<User>() { admin, mod, user };
           
            return users;
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role { Name = "Admin" },
                new Role { Name = "Moderator" },
                new Role { Name = "User"},
            };
            return roles;
        }

        private IEnumerable<Product> GetProducts()
        {
            var products = new List<Product>()
            {
                new Product { Name = "Pineapple", AmountPer = 100, Calories = 54, Carbohydrate=11.8, Protein=0.8, TotalFat=0.2, Verified=true  },
                new Product { Name = "Watermelon", AmountPer = 100, Calories = 36, Carbohydrate=8.4, Protein=0.6, TotalFat=0.1,Verified=true   },
                new Product { Name = "Banana", AmountPer = 100, Calories = 95, Carbohydrate=23.5, Protein=1, TotalFat=0.3, Verified=true   },
                new Product { Name = "Broccoli", AmountPer = 100, Calories = 27, Carbohydrate=5.2, Protein=3, TotalFat=0.4, Verified=true   },
                new Product { Name = "Brussels sprouts", AmountPer = 100, Calories = 37, Carbohydrate=8.7, Protein=4.7, TotalFat=0.5, Verified=true   },
                new Product { Name = "Peach", AmountPer = 100, Calories = 46, Carbohydrate=11.9, Protein=1, TotalFat=0.2,Verified=true   }, 
                new Product { Name = "Kaiser rolls", AmountPer = 100, Calories = 296, Carbohydrate=59.4, Protein=7.5, TotalFat=3.6,Verified=true   },
                new Product { Name = "Butter rolls", AmountPer = 100, Calories = 326, Carbohydrate=61.8, Protein=7.7, TotalFat=5.9,Verified=true   },
                new Product { Name = "Beetroot", AmountPer = 100, Calories = 38, Carbohydrate=9.5, Protein=1.8, TotalFat=0.1,Verified=true   }, 
                new Product { Name = "Onion", AmountPer = 100, Calories = 30, Carbohydrate=6.9, Protein=1.4, TotalFat=0.4,Verified=true   },
                new Product { Name = "Bread", AmountPer = 100, Calories = 245, Carbohydrate=57, Protein=5.4, TotalFat=1.3,Verified=true   },
                new Product { Name = "Whole grain rye bread", AmountPer = 100, Calories = 237, Carbohydrate=53.9, Protein=6.7, TotalFat=1.8,Verified=true   }, 
                new Product { Name = "Corn crisps", AmountPer = 100, Calories = 352, Carbohydrate=78.9, Protein=8.9, TotalFat=3,Verified=true   },
                new Product { Name = "Sugar", AmountPer = 100, Calories = 405, Carbohydrate=99.8, Protein=0, TotalFat=0,Verified=true   },
                new Product { Name = "Lemon", AmountPer = 100, Calories = 36, Carbohydrate=9.5, Protein=0.8, TotalFat=0.3,Verified=true   },
                new Product { Name = "Milk chocolate", AmountPer = 100, Calories = 549, Carbohydrate=54.7, Protein=9.8, TotalFat=32.8,Verified=true   },
                new Product { Name = "Garlic", AmountPer = 100, Calories = 146, Carbohydrate=32.6, Protein=6.4, TotalFat=0.5,Verified=true   },
                new Product { Name = "Pumpkin", AmountPer = 100, Calories = 28, Carbohydrate=7.7, Protein=1.3, TotalFat=0.3,Verified=true   },
                new Product { Name = "Grapefruit", AmountPer = 100, Calories = 36, Carbohydrate=9.8, Protein=0.6, TotalFat=0.2,Verified=true   },
                new Product { Name = "Pear", AmountPer = 100, Calories = 54, Carbohydrate=14.4, Protein=0.6, TotalFat=0.2,Verified=true   },
                new Product { Name = "Biscuits", AmountPer = 100, Calories = 437, Carbohydrate=76.8, Protein=8.2, TotalFat=11,Verified=true   },
                new Product { Name = "Eggs", AmountPer = 100, Calories = 139, Carbohydrate=0.6, Protein=12.5, TotalFat=9.7,Verified=true   },
                new Product { Name = "2% natural yoghurt", AmountPer = 100, Calories = 60, Carbohydrate=6.2, Protein=4.3, TotalFat=2,Verified=true   },
                new Product { Name = "Duck, carcass", AmountPer = 100, Calories = 308, Carbohydrate=0, Protein=13.5, TotalFat=28.6,Verified=true   },
                new Product { Name = "Cauliflower", AmountPer = 100, Calories = 22, Carbohydrate=5, Protein=2.4, TotalFat=0.2,Verified=true   },
                new Product { Name = "White cabbage", AmountPer = 100, Calories = 29, Carbohydrate=7.4, Protein=1.7, TotalFat=0.2,Verified=true   },
                new Product { Name = "Buckwheat groats", AmountPer = 100, Calories = 336, Carbohydrate=69.3, Protein=12.6, TotalFat=3.1,Verified=true   },
                new Product { Name = "Kefir 2%", AmountPer = 100, Calories = 51, Carbohydrate=4.7, Protein=3.4, TotalFat=2,Verified=true   },
                new Product { Name = "Ketchup", AmountPer = 100, Calories = 93, Carbohydrate=22.2, Protein=1.8, TotalFat=1,Verified=true   },
                new Product { Name = "Sausage", AmountPer = 100, Calories = 209, Carbohydrate=0, Protein=17.6, TotalFat=15.6,Verified=true   },
                new Product { Name = "Noodles", AmountPer = 100, Calories = 169, Carbohydrate=32.1, Protein=2.6, TotalFat=3.8,Verified=true   },
                new Product { Name = "Dill", AmountPer = 100, Calories = 26, Carbohydrate=6.1, Protein=2.8, TotalFat=0.4,Verified=true   },
                new Product { Name = "Pork chop", AmountPer = 100, Calories = 351, Carbohydrate=15.9, Protein=19, TotalFat=24.1,Verified=true   },
                new Product { Name = "Fried chicken", AmountPer = 100, Calories = 248, Carbohydrate=7.6, Protein=19.7, TotalFat=15.7,Verified=true  },
                new Product { Name = "Rabbit, carcass", AmountPer = 100, Calories = 156, Carbohydrate=0, Protein=21, TotalFat=8,Verified=true   },
                new Product { Name = "Corn", AmountPer = 100, Calories = 110, Carbohydrate=23.4, Protein=3.7, TotalFat=1.5,Verified=true   },
                new Product { Name = "Canned corn", AmountPer = 100, Calories = 102, Carbohydrate=23.6, Protein=2.9, TotalFat=1.2,Verified=true   },
                new Product { Name = "Hen, carcass", AmountPer = 100, Calories = 202, Carbohydrate=0, Protein=18.5, TotalFat=14.3,Verified=true   },
                new Product { Name = "Roasted chicken", AmountPer = 100, Calories = 179, Carbohydrate=0.1, Protein=16.4, TotalFat=12.7,Verified=true   },
                new Product { Name = "Ice cream", AmountPer = 100, Calories = 160, Carbohydrate=17.6, Protein=3.2, TotalFat=8.5,Verified=true   },
                new Product { Name = "Fresh salmon", AmountPer = 100, Calories = 201, Carbohydrate=0, Protein=19.9, TotalFat=13.6,Verified=true   },
                new Product { Name = "Mayonnaise", AmountPer = 100, Calories = 714, Carbohydrate=2.6, Protein=1.3, TotalFat=79,Verified=true   },
                new Product { Name = "Pasta", AmountPer = 100, Calories = 373, Carbohydrate=77.4, Protein=11.5, TotalFat=2.6,Verified=true   },
                new Product { Name = "Fresh mackerel", AmountPer = 100, Calories = 181, Carbohydrate=0, Protein=18.7, TotalFat=11.9,Verified=true   },
                new Product { Name = "Raspberries", AmountPer = 100, Calories = 29, Carbohydrate=12, Protein=1.3, TotalFat=0.3,Verified=true   },
                new Product { Name = "Carrot", AmountPer = 100, Calories = 27, Carbohydrate=8.7, Protein=1, TotalFat=0.2,Verified=true   },
                new Product { Name = "Butter", AmountPer = 100, Calories = 735, Carbohydrate=0.7, Protein=0.7, TotalFat=82.5,Verified=true   },
                new Product { Name = "Wheat flour", AmountPer = 100, Calories = 342, Carbohydrate=74.9, Protein=9.2, TotalFat=1.2,Verified=true   },
                new Product { Name = "Bee honey", AmountPer = 100, Calories = 324, Carbohydrate=79.5, Protein=0, TotalFat=0,Verified=true   },
                new Product { Name = "Milk 2% ", AmountPer = 100, Calories = 51, Carbohydrate=4.9, Protein=3.4, TotalFat=2,Verified=true   },
                new Product { Name = "Milk 3%", AmountPer = 100, Calories = 61, Carbohydrate=4.8, Protein=3.3, TotalFat=3,Verified=true   },
                new Product { Name = "Whole milk powder", AmountPer = 100, Calories = 479, Carbohydrate=38.7, Protein=27, TotalFat=24,Verified=true   },
                new Product { Name = "Muesli with dried fruit", AmountPer = 100, Calories = 325, Carbohydrate=72.2, Protein=8.4, TotalFat=3.4,Verified=true   },
                new Product { Name = "Mustard", AmountPer = 100, Calories = 162, Carbohydrate=22, Protein=5.7, TotalFat=6.4,Verified=true   },
                new Product { Name = "Cucumber", AmountPer = 100, Calories = 15, Carbohydrate=2.9, Protein=0.7, TotalFat=0.1,Verified=true   },
                new Product { Name = "Oil", AmountPer = 100, Calories = 900, Carbohydrate=0, Protein=0, TotalFat=100,Verified=true   },
                new Product { Name = "Olives", AmountPer = 100, Calories = 147, Carbohydrate=4.1, Protein=1.4, TotalFat=13.9,Verified=true   },
                new Product { Name = "Peanuts", AmountPer = 100, Calories = 634, Carbohydrate=13.3, Protein=26, TotalFat=53,Verified=true   },
                new Product { Name = "Red pepper", AmountPer = 100, Calories = 35, Carbohydrate=6.9, Protein=1.2, TotalFat=0.3,Verified=true   },
                new Product { Name = "Popular sausages", AmountPer = 100, Calories = 347, Carbohydrate=0, Protein=9.5, TotalFat=34.3,Verified=true   },
                new Product { Name = "Popular pate", AmountPer = 100, Calories = 405, Carbohydrate=2, Protein=13.2, TotalFat=38.2,Verified=true   },
                new Product { Name = "Pumpkin seeds", AmountPer = 100, Calories = 596, Carbohydrate=15, Protein=29, TotalFat=46.7,Verified=true   },
                new Product { Name = "Fresh mushroom", AmountPer = 100, Calories = 24, Carbohydrate=2.5, Protein=2.6, TotalFat=0.4,Verified=true   },
                new Product { Name = "Russian frozen dumplings", AmountPer = 100, Calories = 215, Carbohydrate=37.9, Protein=7.2, TotalFat=3.8,Verified=true   },
                new Product { Name = "Frozen dumplings with meat", AmountPer = 100, Calories = 255, Carbohydrate=34.8, Protein=10.5, TotalFat=8.2,Verified=true   },
                new Product { Name = "Parsley root", AmountPer = 100, Calories = 83, Carbohydrate=17.1, Protein=2.6, TotalFat=0.5,Verified=true   },
                new Product { Name = "Cornflakes", AmountPer = 100, Calories = 385, Carbohydrate=83.6, Protein=6.9, TotalFat=2.5,Verified=true   },
                new Product { Name = "Tomato", AmountPer = 100, Calories = 29, Carbohydrate=5.2, Protein=0.9, TotalFat=0.5,Verified=true   },
                new Product { Name = "White rice", AmountPer = 100, Calories = 349, Carbohydrate=78.9, Protein=6.7, TotalFat=0.7,Verified=true   },
                new Product { Name = "Radish", AmountPer = 100, Calories = 21, Carbohydrate=3.6, Protein=0.8, TotalFat=0.4,Verified=true   },
                new Product { Name = "Lettuce", AmountPer = 100, Calories = 20, Carbohydrate=2.9, Protein=1.4, TotalFat=0.3,Verified=true   },
                new Product { Name = "Raw pork loin", AmountPer = 100, Calories = 174, Carbohydrate=0, Protein=21, TotalFat=10,Verified=true   },
                new Product { Name = "Feta cheese", AmountPer = 100, Calories = 216, Carbohydrate=1, Protein=17, TotalFat=16,Verified=true   },
                new Product { Name = "Gouda cheese", AmountPer = 100, Calories = 314, Carbohydrate=1, Protein=27.9, TotalFat=22,Verified=true   },
                new Product { Name = "Fatty processed cheese", AmountPer = 100, Calories = 222, Carbohydrate=1.6, Protein=18, TotalFat=16,Verified=true   },
                new Product { Name = "Semi-fat cottage cheese", AmountPer = 100, Calories = 132, Carbohydrate=3.7, Protein=18.7, TotalFat=4.7,Verified=true   },
                new Product { Name = "Spinach", AmountPer = 100, Calories = 26, Carbohydrate=3, Protein=2.6, TotalFat=0.4,Verified=true   },
                new Product { Name = "Cream 12% fat", AmountPer = 100, Calories = 134, Carbohydrate=3.9, Protein=2.7, TotalFat=12,Verified=true   },
                new Product { Name = "Herring fresh", AmountPer = 100, Calories = 162, Carbohydrate=0, Protein=16.3, TotalFat=10.7,Verified=true   },
                new Product { Name = "Plums", AmountPer = 100, Calories = 70, Carbohydrate=15.9, Protein=1, TotalFat=0.3,Verified=true   },
                new Product { Name = "Strawberries", AmountPer = 100, Calories = 28, Carbohydrate=7.2, Protein=0.7, TotalFat=0.4,Verified=true   },
                new Product { Name = "Tuna in oil", AmountPer = 100, Calories = 190, Carbohydrate=0, Protein=27.1, TotalFat=9,Verified=true   },
                new Product { Name = "Pork liver", AmountPer = 100, Calories = 130, Carbohydrate=2.6, Protein=22, TotalFat=3.4,Verified=true   },
                new Product { Name = "Grapes", AmountPer = 100, Calories = 69, Carbohydrate=17.6, Protein=0.5, TotalFat=0.2,Verified=true   },
                new Product { Name = "Coconut shrims", AmountPer = 100, Calories = 606, Carbohydrate=27, Protein=5.6, TotalFat=63.2,Verified=true   },
                new Product { Name = "Cherries", AmountPer = 100, Calories = 47, Carbohydrate=10.9, Protein=0.9, TotalFat=0.4,Verified=true   },
                new Product { Name = "Beef", AmountPer = 100, Calories = 117, Carbohydrate=0, Protein=20.9, TotalFat=3.6,Verified=true   },
                new Product { Name = "Potatoes", AmountPer = 100, Calories = 85, Carbohydrate=20.5, Protein=1.9, TotalFat=0.1,Verified=true   },
            };

            return products;
        }

    }
}
