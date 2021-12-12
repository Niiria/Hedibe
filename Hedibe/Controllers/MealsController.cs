using Hedibe.Entities;
using Hedibe.Models;
using Hedibe.Models.Meals;
using Hedibe.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Controllers
{
    public class MealsController : Controller
    {


        private readonly HedibeDbContext _context;
        private readonly IUserContextService _userContextService;
        public MealsController(HedibeDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        private static SearchEngineHelper searchHelper = new();
        private static List<Product> MealProducts = new();

        // GET: MealsController
        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            List<Meal> mealsTable = new();

            if (searchHelper.SearchValue is null)
                mealsTable = _context.Meals.Include(m => m.Products).ToList();
            else
            {
                ViewBag.SearchString = searchHelper.SearchValue;
                mealsTable = _context.Meals.Include(m => m.Products).Where(m => m.Name.ToLower().Contains(searchHelper.SearchValue.ToLower())).ToList();
            }


            if (searchHelper.SortString is not null)
                mealsTable = SortMealsTable(searchHelper.SortString, searchHelper.SortDirection, mealsTable);


            int pageSize = 6;
            if (page < 1)
                page = 1;

            int mealsTotal = mealsTable.Count();

            var pager = new Pager(mealsTotal, page, pageSize);

            int mealsSkip = (page - 1) * pageSize;

            var data = mealsTable.Skip(mealsSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;
            ViewData["redirect"] = "Index";

            return View(data);
        }

        [HttpGet]
        public ActionResult UserMeals(int page = 1)
        {
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            List<Meal> mealsTable = new();

            if (searchHelper.SearchValue is null)
                mealsTable = _context.Meals.Include(m => m.Products).Where(p => p.OwnerId == _userContextService.GetUserId()).ToList();
            else
            {
                ViewBag.SearchString = searchHelper.SearchValue;
                mealsTable = _context.Meals.Include(m => m.Products).Where(p => p.OwnerId == _userContextService.GetUserId()).Where(p => p.Name.ToLower().Contains(searchHelper.SearchValue.ToLower())).ToList();
            }

            if (searchHelper.SortString is not null)
                mealsTable = SortMealsTable(searchHelper.SortString, searchHelper.SortDirection, mealsTable);

            int pageSize = 6;
            if (page < 1)
                page = 1;

            int mealsTotal = mealsTable.Count();

            var pager = new Pager(mealsTotal, page, pageSize);

            int mealsSkip = (page - 1) * pageSize;

            var data = mealsTable.Skip(mealsSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;
            ViewData["redirect"] = "UserMeals";

            return View(data);
        }

        // GET: MealsController/Create
        [HttpGet]
        public ActionResult Create(MealAddDto model, bool reset = false)
        {
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            List<Product> productsTable = new();

            if (reset)
                MealProducts = new();

            if (model.Products is null)
            {
                productsTable = _context.Products.ToList();
                if (productsTable is not null)
                    model.Products = productsTable;
            }

            model.CurrentProducts = MealProducts;
            ViewData["redirect"] = "Create";

            return View(model);
        }

        public ActionResult AddSearchProduct(MealAddDto model, string redirect)
        {
            if (MealProducts.FirstOrDefault(p=>p.Id == model.ProductId) is not null)
            {
                TempData["AddInfo"] = "Failed, your product already is in the list!";
                return RedirectToAction(redirect, model);
            }

            var productFromDb = _context.Products.FirstOrDefault(p => p.Id == model.ProductId);
            if (productFromDb is null)
            {
                TempData["AddInfo"] = "Failed, product not found in the database!";
                return RedirectToAction(redirect, model);
            }
                
            MealProducts.Add(productFromDb);
            return RedirectToAction(redirect, model);
        }

        public ActionResult RemoveSearchProduct(MealAddDto model, int itemId, string redirect)
        {
            var itemToRemove = MealProducts.FirstOrDefault(p => p.Id == itemId);
            if (itemToRemove is not null)
                MealProducts.Remove(itemToRemove);
            return RedirectToAction(redirect, model);
        }

        // POST: MealsController/Create
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreateMeal(MealAddDto model)
        {
            ViewBag.AddInfo = null;
            if (ModelState.IsValid)
            {
                Meal MealToDb = new()
                {
                    Name = model.Name,
                    Difficulty = model.Difficulty,
                    Description = model.Description,
                    CookingTime = model.CookingTime,
                    CookingDescription = model.CookingDescription,
                    OwnerId = _userContextService.GetUserId(),
                    Products = new(),
                    Verified = false
                };

                foreach (var item in MealProducts)
                {
                    var itemFromDb = _context.Products.FirstOrDefault(i => i.Id == item.Id);
                    MealToDb.Products.Add(itemFromDb);
                }

                _context.Meals.Attach(MealToDb);
                await _context.SaveChangesAsync();
                MealProducts = new();
                TempData["AddInfo"] = "Succesfully created your meal!";
                return RedirectToAction("Create");
            }
            TempData["AddInfo"] = "Failed to created your meal!";
            return RedirectToAction("Create", model);
        }


        public ActionResult Details(int? Id, string redirect)
        {
            var model = _context.Meals.Include(m => m.Products).FirstOrDefault(m => m.Id == Id);
            if (model is null)
                RedirectToAction("Index");

            ViewData["redirect"] = redirect;

            return View(model);
        }


        // GET: Meals/Edit/5

        public ActionResult Edit(MealAddDto model)
        {
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            ViewData["redirect"] = "Edit";

            if (model.Name is null && model.Description is null)
            {
                var mealFromDb = _context.Meals.Include(p => p.Products).FirstOrDefault(m => m.Id == model.Id);
                if (mealFromDb is null)
                    return RedirectToAction("Add");

                MealAddDto mealToUpdate = new()
                {
                    Id = mealFromDb.Id,
                    Name = mealFromDb.Name,
                    Difficulty = mealFromDb.Difficulty,
                    Description = mealFromDb.Description,
                    CookingTime = mealFromDb.CookingTime,
                    CookingDescription = mealFromDb.CookingDescription,
                    OwnerId = mealFromDb.OwnerId,
                    Verified = mealFromDb.Verified,
                    CurrentProducts = mealFromDb.Products,
                    Products = _context.Products.ToList()
                };

                MealProducts = mealFromDb.Products;
                return View(mealToUpdate);
            }

            if (model.Products is null)
            {
                var productsTable = _context.Products.ToList();
                if (productsTable is not null)
                    model.Products = productsTable;
            }
            model.CurrentProducts = MealProducts;


            return View(model);

        }


        // POST: Meals/Edit/5
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditMeal(MealAddDto model)
        {
            var MealFromDb = _context.Meals.Include(p => p.Products).FirstOrDefault(m => m.Id == model.Id);
            if (MealFromDb is null)
                return RedirectToAction("Edit");

            ViewBag.AddInfo = null;
            if (ModelState.IsValid)
            {
                _context.Meals.Attach(MealFromDb);

                MealFromDb.Name = model.Name;
                MealFromDb.Difficulty = model.Difficulty;
                MealFromDb.Description = model.Description;
                MealFromDb.CookingTime = model.CookingTime;
                MealFromDb.CookingDescription = model.CookingDescription;
                MealFromDb.Products.Clear();

                foreach (var item in MealProducts)
                {
                    var itemFromDb = _context.Products.FirstOrDefault(i => i.Id == item.Id);
                    MealFromDb.Products.Add(itemFromDb);
                }

                await _context.SaveChangesAsync();
                TempData["AddInfo"] = "Succesfully updated your meal!";
                return RedirectToAction("Edit", model);
            }
            TempData["AddInfo"] = "Failed to add your meal!";
            return RedirectToAction("Edit", model);
        }

        // GET: Meals/Delete/5
        public async Task<IActionResult> Delete(int id, string redirect)
        {
            try
            {
                var mealFromDb = _context.Meals.FirstOrDefault(m => m.Id == id);
                if (mealFromDb is null)
                    return RedirectToAction("Index");

                _context.Meals.Remove(mealFromDb);
                await _context.SaveChangesAsync();
                TempData["AddInfo"] = "Succesfully deleted meal!";
                return RedirectToAction(redirect);
            }
            catch (Exception)
            {
                TempData["AddInfo"] = "Failed to delete meal!";
                return RedirectToAction("Details", id);
            }
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

                default:
                    break;
            }
            return list;
        }
    }
}
