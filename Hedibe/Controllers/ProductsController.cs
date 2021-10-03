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

        // private static List<Product> ProductsTable = new();

        // GET: ProductsController
        public ActionResult Index(int page=1)
        {
            List<Product> productsTable = _context.Products.ToList();

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

     

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Render(string? id)
        {
            return RedirectToAction("Add", "Products", new { @id = id });
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
    }
}
