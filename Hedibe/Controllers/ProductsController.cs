using Hedibe.Entities;
using Hedibe.Models;
using Hedibe.Models.Product;
using Hedibe.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HedibeDbContext _context;
        private readonly IUserContextService _userContextService;
        public ProductsController(HedibeDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        // private static List<Product> productsTable = new();

        private static string lastSortString = null;
        private static string sortString = null;
        private static bool sortDirection = true;
        private static string searchValue = null;

        

        // GET: ProductsController
        public ActionResult Index(int page=1)
        {
            List<Product> productsTable = new();

            if (searchValue is null)
                productsTable = _context.Products.ToList();
            else
            {
                ViewBag.SearchString = searchValue;
                productsTable = _context.Products.Where(p => p.Name.ToLower().Contains(searchValue.ToLower())).ToList();
            }
                

            if (sortString is not null)
                productsTable = SortProductsTable(sortString, sortDirection ,productsTable);


            int pageSize = 10;
            if (page < 1)
                page = 1;

            int productsTotal = productsTable.Count();

            var pager = new Pager(productsTotal, page, pageSize);

            int productsSkip = (page - 1) * pageSize;

            var data = productsTable.Skip(productsSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

           
            return View(data);
        }

     

       

        [HttpGet]
        // GET: ProductsController/Create
        public ActionResult Add(string message)
        {
            if(message is not null)
                ViewBag.AddInfo = message;
            else
                ViewBag.AddInfo = null;

            return View();
        }


        // POST: ProductsController/Create
        [HttpPost, ActionName("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(ProductAddDto dto)
        {
            ViewBag.AddInfo = null;
            if (ModelState.IsValid)
            {
                Product productToDb = new()
                {
                    Name = dto.Name,
                    AmountPer = dto.AmountPer,
                    Calories = dto.Calories,
                    TotalFat = dto.TotalFat,
                    Protein = dto.Protein,
                    Carbohydrate = dto.Carbohydrate,
                    Verified = false,
                    OwnerId = _userContextService.GetUserId()
                };

                _context.Products.Add(productToDb);
                await _context.SaveChangesAsync();
                return RedirectToAction("Add", new {@message = "Succesfully added your product!" });

            }
            return View();
        }

        public IActionResult Search(string? searchString, bool reset, bool verified)
        {
            searchValue = searchString;

            if (searchString == "" || reset) 
                searchValue = null;
                sortString = null;
            
            return RedirectToAction("Index");
        }

        public IActionResult Sort(string? sort)
        {
            sortString = sort;
            if (sortString == lastSortString)
            {
                sortDirection = false;
                lastSortString = null;
            }
            else
            {
                sortDirection = true;
                lastSortString = sortString;
            }

            return RedirectToAction("Index");
        }




        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductsController/Edit/5
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

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductsController/Delete/5
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

        private List<Product> SortProductsTable(string sortStr, bool sortDir ,List<Product> list)
        {
            switch (sortString)
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
    }
}
