using Hedibe.Entities;
using Hedibe.Models;
using Hedibe.Models.Meals;
using Hedibe.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


            int pageSize = 9;
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

            int pageSize = 10;
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
            var productFromDb = _context.Products.FirstOrDefault(p => p.Id == model.ProductId);
            if (productFromDb is not null)
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
                TempData["AddInfo"] = "Succesfully created your meal!";
                return RedirectToAction("Create", model);
            }
            TempData["AddInfo"] = "Failed to created your meal!";
            return RedirectToAction("Create", model);
        }


        public ActionResult Details(int? Id)
        {
            var model = _context.Meals.Include(m => m.Products).FirstOrDefault(m => m.Id == Id);
            if (model is null)
                RedirectToAction("Index");
            return View(model);
        }


        // GET: MealsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MealsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MealsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MealsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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

                /*   case "Carbohydrate":
                       if (sortDir)
                           return list.OrderBy(o => o.Carbohydrate).ToList();
                       else
                           return list.OrderByDescending(o => o.Carbohydrate).ToList();*/

                default:
                    break;
            }
            return list;
        }
    }
}
