using Hedibe.Entities;
using Hedibe.Models.ShoppingLists;
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
    public class ShoppingListController : Controller
    {
        private readonly HedibeDbContext _context;
        private readonly IUserContextService _userContextService;

        public ShoppingListController(HedibeDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        private static List<Product> ShoppingListProducts = new();

        public ActionResult Index()
        {
            List<ShoppingList> shoppingLists = new();

            shoppingLists = _context.ShoppingLists.Include(p => p.Products).Where(sl => sl.OwnerId == _userContextService.GetUserId()).ToList();

            return View(shoppingLists);
        }

        [HttpGet]
        public ActionResult Add(ShoppingListAddDto model)
        {
            List<Product> productsTable = new();
            // ShoppingListAddModel = model;
            if(model.Products is null)
            {
                productsTable = _context.Products.ToList();
                if (productsTable is not null)
                    model.Products = productsTable;
            }
            model.CurrentProducts = ShoppingListProducts;

            return View(model);
        }

        public ActionResult AddSearchProduct(ShoppingListAddDto model)
        {
            var productFromDb = _context.Products.FirstOrDefault(p => p.Id == model.ProductId);
            if (productFromDb is not null)
                ShoppingListProducts.Add(productFromDb);
            return RedirectToAction("Add", model);
        }

        public ActionResult RemoveSearchProduct(int? id, ShoppingListAddDto model)
        {
            var itemToRemove = ShoppingListProducts.FirstOrDefault(p => p.Id == id);
            if(itemToRemove is not null)
                ShoppingListProducts.Remove(itemToRemove);
            return RedirectToAction("Add", model);
        }

        [HttpPost, ActionName("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddList(ShoppingListAddDto model)
        {
            if (ModelState.IsValid)
            {
                ShoppingList shoppingListToDb = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    OwnerId = _userContextService.GetUserId(),
                    Products = model.CurrentProducts
                };

                _context.ShoppingLists.Attach(shoppingListToDb);
                ShoppingListProducts = new();

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
