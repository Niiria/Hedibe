using Hedibe.Entities;
using Hedibe.Models;
using Hedibe.Models.Products;
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
            ViewData["redirect"] = "Index";

            return View(data);
        }

        [HttpGet]
        public ActionResult UserProducts(int page = 1)
        {
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            List<Product> productsTable = new();

            if (searchValue is null)
                productsTable = _context.Products.Where(p => p.OwnerId == _userContextService.GetUserId()).ToList();
            else
            {
                ViewBag.SearchString = searchValue;
                productsTable = _context.Products.Where(p => p.OwnerId == _userContextService.GetUserId()).Where(p => p.Name.ToLower().Contains(searchValue.ToLower())).ToList();
            }

            if (sortString is not null)
                productsTable = SortProductsTable(sortString, sortDirection, productsTable);

            int pageSize = 10;
            if (page < 1)
                page = 1;

            int productsTotal = productsTable.Count();

            var pager = new Pager(productsTotal, page, pageSize);

            int productsSkip = (page - 1) * pageSize;

            var data = productsTable.Skip(productsSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;
            ViewData["redirect"] = "UserProducts";

            return View(data);
        }



        [HttpGet]
        // GET: ProductsController/Create
        public ActionResult Add(ProductAddDto model)
        {
            if(TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            return View(model);
        }


        // POST: ProductsController/Create
        [HttpPost, ActionName("Add")]
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
                TempData["AddInfo"] = "Succesfully added your product!";
                return RedirectToAction("Add");
            }
            TempData["AddInfo"] = "Failed to add your product!";
            return RedirectToAction("Add", dto);
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id, string redirect, int? page)
        {
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            var productFromDb = _context.Products.FirstOrDefault(p => p.Id == id);
            if (productFromDb is null)
                return RedirectToAction("Add");


            if (redirect is not null)
                ViewData["redirect"] = redirect;
            if (page is not null)
                ViewData["page"] = page;

            return View(productFromDb);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Product dto, string redirect, int? page)
        {
            var productFromDb = _context.Products.FirstOrDefault(p => p.Id == dto.Id);
            if (productFromDb is null)
                return RedirectToAction("Add");

            ViewBag.AddInfo = null;
            if (ModelState.IsValid)
            {
                productFromDb.Name = dto.Name;
                productFromDb.AmountPer = dto.AmountPer;
                productFromDb.Calories = dto.Calories;
                productFromDb.TotalFat = dto.TotalFat;
                productFromDb.Protein = dto.Protein;
                productFromDb.Carbohydrate = dto.Carbohydrate;
                productFromDb.Verified = false;
                productFromDb.OwnerId = _userContextService.GetUserId();


                _context.Products.Update(productFromDb);
                await _context.SaveChangesAsync();

                if (redirect is not null)
                {
                    if (redirect == "EditProducts")
                        return RedirectToAction(redirect, "Panel", new { @page = page });
                    return RedirectToAction(redirect, new { @page = page });
                }

                TempData["AddInfo"] = "Succesfully updated your product!";
                return RedirectToAction("Edit", new { @id = productFromDb.Id, });
            }
            return View();
        }

        // POST: ProductsController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var productFromDb = _context.Products.FirstOrDefault(sl => sl.Id == id);
                if (productFromDb is null)
                    return RedirectToAction("Index");

                _context.Products.Remove(productFromDb);
                await _context.SaveChangesAsync();
                TempData["AddInfo"] = "Succesfully deleted your product!";
                return RedirectToAction("UserProducts");
            }
            catch (Exception)
            {
                TempData["AddInfo"] = "Failed to delete your product!";
                return RedirectToAction("UserProducts");
            }
        }

        public IActionResult Search(string searchString, bool reset, string redirect)
        {
            searchValue = searchString;

            if (searchString == "" || reset) 
                searchValue = null;
                sortString = null;
            
            return RedirectToAction(redirect);
        }

        public IActionResult Sort(string sort, string redirect)
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

            return RedirectToAction(redirect);
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
