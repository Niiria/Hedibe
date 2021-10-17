using Hedibe.Entities;
using Hedibe.Models.ShoppingLists;
using Hedibe.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Controllers
{
    public class ShoppingListController : Controller
    {
        private readonly HedibeDbContext _context;
        private readonly IUserContextService _userContextService;

        public ShoppingListController(HedibeDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        private static List<Product> shoppingListProducts = new();

        public ActionResult Index()
        {
            List<ShoppingList> shoppingLists = new();

            shoppingLists = _context.ShoppingLists.Where(sl => sl.OwnerId == _userContextService.GetUserId()).ToList();

            return View(shoppingLists);
        }


        public ActionResult Add()
        {
            List<Product> productsTable = new();
            ProductsList searchProducts = new();

            productsTable = _context.Products.ToList();
            if (productsTable is not null)
                searchProducts.Products = productsTable;


            searchProducts.CurrentProducts = shoppingListProducts;
            ViewBag.searchProducts = searchProducts;
            return View();
        }

        public ActionResult AddSearchProduct(int? Id)
        {
            var productFromDb = _context.Products.FirstOrDefault(p => p.Id == Id);
            shoppingListProducts.Add(productFromDb);
            return RedirectToAction("Add");
        }

        public ActionResult RemoveSearchProduct(int? Id)
        {
            var itemToRemove = shoppingListProducts.FirstOrDefault(p => p.Id == Id);
            if(itemToRemove is not null) { }
                shoppingListProducts.Remove(itemToRemove);
            return RedirectToAction("Add");
        }



        // POST: ShoppingListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ShoppingListAddDto model)
        {
            if (ModelState.IsValid)
            {
                ShoppingList shoppingListToDb = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    OwnerId = _userContextService.GetUserId(),
                    Products = shoppingListProducts
                };

                _context.ShoppingLists.Attach(shoppingListToDb);

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Add");
        }




        // GET: ShoppingListController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ShoppingListController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShoppingListController/Edit/5
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

        // GET: ShoppingListController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShoppingListController/Delete/5
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
