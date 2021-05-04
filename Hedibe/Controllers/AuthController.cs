using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hedibe.Data;
using Hedibe.Models;
using System.ComponentModel.DataAnnotations;

namespace Hedibe.Controllers
{
    public class AuthController : Controller
    {
        private readonly SqlContext _context;
        public static User LoggedUser { get; set; } = null;


        [BindProperties]
        public class InputModel
        {
            [Required]
            public string Username { get; set; }
            [Required, DataType(DataType.Password)]
            public string Password { get; set; } 
            [Required, Compare("Password"), DataType(DataType.Password)]
            public string ConfirmPassword { get; set; } 
        }

        public AuthController(SqlContext context)
        {
            _context = context;
        }

        // GET: Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // GET: Auth/Login
        public IActionResult Login(InputModel model)
        {
            ModelState.Clear();
            return View(model);
        }

        // GET: Auth/Logout
        public IActionResult Logout()
        {
            LoggedUser = null;
            return RedirectToAction("Index", "Home");
        }


        // POST: Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(InputModel model)
        {
            if (ModelState.IsValid)
            {
                User UserToDb = new() { Username = model.Username, Password = model.Password };
               //_context.Add(UserToDb);
               // await _context.SaveChangesAsync();
                return RedirectToAction("Login", model);
            }
            return View(model);
        }

        // POST: Auth/Login
        [ActionName("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login_submit(InputModel inputUser)
        {
            LoggedUser = new() {
                Username= inputUser.Username
            };
            return View();
        }
    }
}
