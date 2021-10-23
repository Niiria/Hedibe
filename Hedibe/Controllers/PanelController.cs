using Hedibe.Entities;
using Hedibe.Models;
using Hedibe.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Controllers
{
    public class PanelController : Controller
    {
        private readonly HedibeDbContext _context;
        private readonly IUserContextService _userContextService;
        public PanelController(HedibeDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }

        private static string lastSortString = null;
        private static string sortString = null;
        private static bool sortDirection = true;
        private static string searchValue = null;

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult VerifyProducts(int page = 1)
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
                productsTable = SortProductsTable(sortString, sortDirection, productsTable);


            int pageSize = 10;
            if (page < 1)
                page = 1;

            int productsTotal = productsTable.Count();

            var pager = new Pager(productsTotal, page, pageSize);

            int productsSkip = (page - 1) * pageSize;

            var data = productsTable.Skip(productsSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;
            ViewData["action"]= "VerifyProducts";


            return View(data);
      
        }




        public async Task<IActionResult> Verify(int? id, string? redirect, int? page)
        {
            var productToVerify = _context.Products.FirstOrDefault(p => p.Id == id);
            if (productToVerify is null)
                return RedirectToAction(redirect);

            productToVerify.Verified = !productToVerify.Verified;
            _context.Products.Update(productToVerify);
            await _context.SaveChangesAsync();

            return RedirectToAction(redirect, new { @page = page });
        }

        public async Task<IActionResult> Delete(int? id, string? redirect)
        {
            var productToVerify = _context.Products.FirstOrDefault(p => p.Id == id);
            if (productToVerify is null)
                return RedirectToAction(redirect);

            _context.Products.Remove(productToVerify);
            await _context.SaveChangesAsync();

            return RedirectToAction(redirect);
        }


        public IActionResult Search(string? searchString, bool reset, string? redirect)
        {
            searchValue = searchString;

            if (searchString == "" || reset)
                searchValue = null;
            sortString = null;

            return RedirectToAction(redirect);
        }

        public IActionResult Sort(string? sort, string? redirect)
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

        private List<Product> SortProductsTable(string sortStr, bool sortDir, List<Product> list)
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
