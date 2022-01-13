using Hedibe.Entities;
using Hedibe.Models;
using Hedibe.Models.Account;
using Hedibe.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Controllers
{
    public class PanelController : Controller
    {
        private readonly HedibeDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly IPasswordHasher<User> _passwordHasher;
        public PanelController(HedibeDbContext context, IUserContextService userContextService, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _userContextService = userContextService;
            _passwordHasher = passwordHasher;
        }

        private static SearchEngineHelper searchHelper = new();
        private static List<Product> MealProducts = new();

        public IActionResult Dashboard()
        {
            if(_userContextService.GrantAccessToRoles("Admin", "Moderator"))
                return View();

            return NotFound();
        }

        public IActionResult VerifyProducts(int page = 1)
        {
            List<Product> productsTable = new();

            if (searchHelper.SearchValue is null)
                productsTable = _context.Products.ToList();
            else
            {
                ViewBag.SearchString = searchHelper.SearchValue;
                productsTable = _context.Products.Where(p => p.Name.ToLower().Contains(searchHelper.SearchValue.ToLower())).ToList();
            }


            if (searchHelper.SortString is not null)
                productsTable = SortProductsTable(searchHelper.SortString, searchHelper.SortDirection, productsTable);


            int pageSize = 10;
            if (page < 1)
                page = 1;

            int productsTotal = productsTable.Count();

            var pager = new Pager(productsTotal, page, pageSize);

            int productsSkip = (page - 1) * pageSize;

            var data = productsTable.Skip(productsSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;
            ViewData["redirect"] = "VerifyProducts";

            return View(data);
        }

        public IActionResult VerifyMeals(int page = 1)
        {
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            List<Meal> mealsTable = new();

            if (searchHelper.SearchValue is null)
                mealsTable = _context.Meals.Include(m => m.Products).ToList();
            else
            {
                ViewBag.SearchString = searchHelper.SearchValue;
                mealsTable = _context.Meals.Include(m => m.Products).Where(p => p.Name.ToLower().Contains(searchHelper.SearchValue.ToLower())).ToList();
            }

            if (searchHelper.SortString is not null)
                mealsTable = SortMealsTable(searchHelper.SortString, searchHelper.SortDirection, mealsTable);

            int pageSize = 10;
            if (page < 1)
                page = 1;

            int mealsTotal = mealsTable.Count();

            var pager = new Pager(mealsTotal, page, pageSize);

            int mealsSkip = (page - 1) * pageSize;

            var data = mealsTable.Skip(mealsSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;
            ViewData["redirect"] = "VerifyMeals";

            return View(data);
        }


        public IActionResult EditProducts(int page = 1)
        {
            List<Product> productsTable = new();

            if (searchHelper.SearchValue is null)
                productsTable = _context.Products.ToList();
            else
            {
                ViewBag.SearchString = searchHelper.SearchValue;
                productsTable = _context.Products.Where(p => p.Name.ToLower().Contains(searchHelper.SearchValue.ToLower())).ToList();
            }


            if (searchHelper.SortString is not null)
                productsTable = SortProductsTable(searchHelper.SortString, searchHelper.SortDirection, productsTable);


            int pageSize = 10;
            if (page < 1)
                page = 1;

            int productsTotal = productsTable.Count();

            var pager = new Pager(productsTotal, page, pageSize);

            int productsSkip = (page - 1) * pageSize;

            var data = productsTable.Skip(productsSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;
            ViewData["redirect"] = "EditProducts";


            return View(data);
        }

        public IActionResult EditMeals(int page = 1)
        {
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            List<Meal> mealsTable = new();

            if (searchHelper.SearchValue is null)
                mealsTable = _context.Meals.Include(m => m.Products).ToList();
            else
            {
                ViewBag.SearchString = searchHelper.SearchValue;
                mealsTable = _context.Meals.Include(m => m.Products).Where(p => p.Name.ToLower().Contains(searchHelper.SearchValue.ToLower())).ToList();
            }

            if (searchHelper.SortString is not null)
                mealsTable = SortMealsTable(searchHelper.SortString, searchHelper.SortDirection, mealsTable);

            int pageSize = 10;
            if (page < 1)
                page = 1;

            int mealsTotal = mealsTable.Count();

            var pager = new Pager(mealsTotal, page, pageSize);

            int mealsSkip = (page - 1) * pageSize;

            var data = mealsTable.Skip(mealsSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;
            ViewData["redirect"] = "EditMeals";

            return View(data);
        }

        public IActionResult EditUsers(int page = 1)
        {
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            List<RegisterUserDto> registerUsersTable = new();
            List<User> userTable = new();

            if (searchHelper.SearchValue is null)
            {
                userTable = _context.Users.Include(r => r.Role).ToList();
                foreach (var item in userTable)
                {
                    registerUsersTable.Add(new RegisterUserDto() { Id = item.Id, Username = item.Username, Email = item.Email, RoleId = item.RoleId });
                }
            }

            else
            {
                ViewBag.SearchString = searchHelper.SearchValue;
                userTable = _context.Users.Include(r => r.Role).Where(p => p.Username.ToLower().Contains(searchHelper.SearchValue.ToLower())).ToList();
                foreach (var item in userTable)
                {
                    registerUsersTable.Add(new RegisterUserDto() { Id = item.Id, Username = item.Username, Email = item.Email, RoleId = item.RoleId });
                }
            }

            int pageSize = 10;
            if (page < 1)
                page = 1;

            int usersTotal = registerUsersTable.Count();

            var pager = new Pager(usersTotal, page, pageSize);

            int usersSkip = (page - 1) * pageSize;

            var data = registerUsersTable.Skip(usersSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;
            ViewData["redirect"] = "EditUsers";

            return View(data);
        }


        public async Task<IActionResult> VerifyProduct(int id, string redirect, int page = 1)
        {
            var productToVerify = _context.Products.FirstOrDefault(p => p.Id == id);
            if (productToVerify is null)
                return RedirectToAction(redirect, new { @page = page });

            productToVerify.Verified = !productToVerify.Verified;
            _context.Products.Update(productToVerify);
            await _context.SaveChangesAsync();

            return RedirectToAction(redirect, new { @page = page });
        }

        public async Task<IActionResult> VerifyMeal(int id, string redirect, int page = 1)
        {
            var mealToVerify = _context.Meals.FirstOrDefault(p => p.Id == id);
            if (mealToVerify is null)
                return RedirectToAction(redirect, new { @page = page });

            mealToVerify.Verified = !mealToVerify.Verified;
            _context.Meals.Update(mealToVerify);
            await _context.SaveChangesAsync();

            return RedirectToAction(redirect, new { @page = page });
        }

        public async Task<IActionResult> DeleteProduct(int id, string redirect, int page=1)
        {
            var productToDelete = _context.Products.FirstOrDefault(p => p.Id == id);
            if (productToDelete is null)
                return RedirectToAction(redirect, new { @page = page });

            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();

            return RedirectToAction(redirect, new { @page = page });
        }

        public async Task<IActionResult> DeleteMeal(int id, string redirect, int page=1)
        {
            var mealToDelete = _context.Meals.FirstOrDefault(p => p.Id == id);
            if (mealToDelete is null)
                return RedirectToAction(redirect, new { @page = page });

            _context.Meals.Remove(mealToDelete);
            await _context.SaveChangesAsync();

            return RedirectToAction(redirect, new { @page = page });
        }

        public async Task<IActionResult> DeleteUser(int id, string redirect, int page = 1)
        {
            var userToDelete = _context.Users.FirstOrDefault(p => p.Id == id);
            if (userToDelete is null)
                return RedirectToAction(redirect, new { @page = page });

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return RedirectToAction(redirect, new { @page = page });
        }


        public IActionResult Search(string searchString, bool reset, string redirect)
        {
            searchHelper.SearchValue = searchString;

            if (searchString == "" || reset)
                searchHelper.SearchValue = null;
            searchHelper.SortString = null;

            return RedirectToAction(redirect);
        }

        public IActionResult Sort(string sort, string redirect)
        {
            searchHelper.SortString = sort;
            if (searchHelper.SortString == searchHelper.LastSortString)
            {
                searchHelper.SortDirection = false;
                searchHelper.LastSortString = null;
            }
            else
            {
                searchHelper.SortDirection = true;
                searchHelper.LastSortString = searchHelper.SortString;
            }

            return RedirectToAction(redirect);
        }

        private List<Product> SortProductsTable(string sortStr, bool sortDir, List<Product> list)
        {
            switch (searchHelper.SortString)
            {
                case "Name":
                    if (sortDir)
                        return list.OrderBy(o => o.Name).ToList();
                    else
                        return list.OrderByDescending(o => o.Name).ToList();

                case "Calories":
                    if (sortDir)
                        return list.OrderBy(o => o.Calories).ToList();
                    else
                        return list.OrderByDescending(o => o.Calories).ToList();

                case "TotalFat":
                    if (sortDir)
                        return list.OrderBy(o => o.TotalFat).ToList();
                    else
                        return list.OrderByDescending(o => o.TotalFat).ToList();

                case "Protein":
                    if (sortDir)
                        return list.OrderBy(o => o.Protein).ToList();
                    else
                        return list.OrderByDescending(o => o.Protein).ToList();

                case "Carbohydrate":
                    if (sortDir)
                        return list.OrderBy(o => o.Carbohydrate).ToList();
                    else
                        return list.OrderByDescending(o => o.Carbohydrate).ToList();

                default:
                    break;
            }
            return list;
        }

        private List<Meal> SortMealsTable(string sortStr, bool sortDir, List<Meal> list)
        {
            switch (searchHelper.SortString)
            {
                case "Name":
                    if (sortDir)
                        return list.OrderBy(o => o.Name).ToList();
                    else
                        return list.OrderByDescending(o => o.Name).ToList();

                case "TotalCalories":
                    if (sortDir)
                        return list.OrderBy(o => o.CalculateTotalCalories()).ToList();
                    else
                        return list.OrderByDescending(o => o.CalculateTotalCalories()).ToList();

                case "Difficulty":
                    if (sortDir)
                        return list.OrderBy(o => o.Difficulty).ToList();
                    else
                        return list.OrderByDescending(o => o.Difficulty).ToList();

                case "CookingTime":
                    if (sortDir)
                        return list.OrderBy(o => o.CookingTime).ToList();
                    else
                        return list.OrderByDescending(o => o.CookingTime).ToList();
                case "Verified":
                    if (sortDir)
                        return list.OrderBy(o => o.Verified).ToList();
                    else
                        return list.OrderByDescending(o => o.Verified).ToList();



                default:
                    break;
            }
            return list;
        }

    }
}
