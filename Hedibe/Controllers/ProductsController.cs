using Hedibe.Entities;
using Hedibe.Models;
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
        public ProductsController(HedibeDbContext context)
        {
            _context = context;
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

        // GET: ProductsController/Create
        public ActionResult Add(string id)
        {
            if (id == null)
                   id = "_Organization";
            this.ViewBag.Render = id;
            return View();
        }

        public ActionResult Render(string? id)
        {
            return RedirectToAction("Add","Products", new {@id = id });
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(IFormCollection collection)
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
