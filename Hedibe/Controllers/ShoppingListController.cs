﻿using Hedibe.Entities;
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
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            List<ShoppingList> shoppingLists = new();
            ShoppingListProducts = new();

            shoppingLists = _context.ShoppingLists.Include(p => p.Products).Where(sl => sl.OwnerId == _userContextService.GetUserId()).ToList();

            return View(shoppingLists);
        }

        public ActionResult AddSearchProduct(ShoppingListAddDto model, string redirect)
        {
            var productFromDb = _context.Products.FirstOrDefault(p => p.Id == model.ProductId);
            if (productFromDb is not null)
                ShoppingListProducts.Add(productFromDb);
            return RedirectToAction(redirect, model);
        }

        public ActionResult RemoveSearchProduct(ShoppingListAddDto model, int itemId, string redirect)
        {
            var itemToRemove = ShoppingListProducts.FirstOrDefault(p => p.Id == itemId);
            if (itemToRemove is not null)
                ShoppingListProducts.Remove(itemToRemove);
            return RedirectToAction(redirect, model);
        }


        // GET: ShoppingListController/Add

        [HttpGet]
        public ActionResult Add(ShoppingListAddDto model, bool reset=false)
        {
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            List<Product> productsTable = new();
           
            if (reset)
                ShoppingListProducts = new();

            if (model.Products is null)
            {
                productsTable = _context.Products.ToList();
                if (productsTable is not null)
                    model.Products = productsTable;
            }

            model.CurrentProducts = ShoppingListProducts;
            ViewData["redirect"] = "Add";

            return View(model);
        }

        // POST: ShoppingListController/Add
        [HttpPost, ActionName("Add")]
        public async Task<IActionResult> AddList(ShoppingListAddDto model)
        {
            if (ModelState.IsValid)
            {
                ShoppingList shoppingListToDb = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    OwnerId = _userContextService.GetUserId(),
                    Products = ShoppingListProducts
                };

                _context.ShoppingLists.Attach(shoppingListToDb);

                await _context.SaveChangesAsync();
                ShoppingListProducts = new();
                TempData["AddInfo"] = "Succesfully added your shopping list!";
                return RedirectToAction("Add");
            }
            TempData["AddInfo"] = "Failed to add your shopping list!";
            return RedirectToAction("Add", model);
        }





        // GET: ShoppingListController/Edit/5

        public ActionResult Edit(ShoppingListAddDto model)
        {
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            ViewData["redirect"] = "Edit";

            if (model.Name is null && model.Description is null )
            {
                var shoppingListFromDb = _context.ShoppingLists.Include(p => p.Products).FirstOrDefault(sl => sl.Id == model.Id);
                if (shoppingListFromDb is null)
                    return RedirectToAction("Add");

                ShoppingListAddDto shoppingListToUpdate = new()
                {
                    Name = shoppingListFromDb.Name,
                    Description = shoppingListFromDb.Description,
                    CurrentProducts = shoppingListFromDb.Products,
                    Products = _context.Products.ToList()
                };

                ShoppingListProducts = shoppingListFromDb.Products;
                return View(shoppingListToUpdate);
            }

            if (model.Products is null)
            {
                var productsTable = _context.Products.ToList();
                if (productsTable is not null)
                    model.Products = productsTable;
            }
            model.CurrentProducts = ShoppingListProducts;


            return View(model);

        }


        // POST: ShoppingListController/Edit/5
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditShoppingList(ShoppingListAddDto dto)
        {
            var shoppingListFromDb = _context.ShoppingLists.Include(p => p.Products).FirstOrDefault(sl => sl.Id == dto.Id);
            if (shoppingListFromDb is null)
                return RedirectToAction("Edit");

            ViewBag.AddInfo = null;
            if (ModelState.IsValid)
            {
                _context.ShoppingLists.Attach(shoppingListFromDb);
            
                shoppingListFromDb.Id = dto.Id;
                shoppingListFromDb.Name = dto.Name;
                shoppingListFromDb.Description = dto.Description;
                shoppingListFromDb.OwnerId = _userContextService.GetUserId();
                shoppingListFromDb.Products.Clear();

                foreach (var item in ShoppingListProducts)
                {
                    var itemFromDb = _context.Products.FirstOrDefault(i=>i.Id == item.Id);
                    shoppingListFromDb.Products.Add(itemFromDb);
                }

                await _context.SaveChangesAsync();
                TempData["AddInfo"] = "Succesfully updated your shopping list!";
                return RedirectToAction("Edit", dto);
            }
            TempData["AddInfo"] = "Failed to add your shopping list!";
            return RedirectToAction("Edit", dto);
        }

        // GET: ShoppingListController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var shoppingListFromDb = _context.ShoppingLists.FirstOrDefault(sl => sl.Id == id);
                if (shoppingListFromDb is null)
                    return RedirectToAction("Index");

                _context.ShoppingLists.Remove(shoppingListFromDb);
                await _context.SaveChangesAsync();
                TempData["AddInfo"] = "Succesfully deleted your shopping list!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["AddInfo"] = "Failed to deleted your shopping list!";
                return RedirectToAction("Index");
            }
        }
    }
}
