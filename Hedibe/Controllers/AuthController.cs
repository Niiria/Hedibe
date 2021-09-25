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
using Microsoft.AspNetCore.Identity;

namespace Hedibe.Controllers
{
    public class AuthController : Controller
    {
        private readonly SqlContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        public static User LoggedUser { get; set; } = null;



        [BindProperties]
        public class RegisterModel
        {
            [Required]
            public string Username { get; set; }
            [Required, DataType(DataType.Password)]
            public string Password { get; set; } 
            [Required, Compare("Password"), DataType(DataType.Password)]
            public string ConfirmPassword { get; set; } 
        }

        public AuthController(SqlContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // GET: Auth/Login
        public IActionResult Login(RegisterModel model)
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
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User UserToDb = new() { Username = model.Username };
                var passwordHash = _passwordHasher.HashPassword(UserToDb, model.Password);
                UserToDb.PasswordHash = passwordHash;

                _context.Users.Add(UserToDb);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", model);
            }
            return View(model);
        }


        // POST: Auth/Login
        [ActionName("Login")]
        [HttpPost]
        public IActionResult Login_submit(RegisterModel model)
        {
            User userFromDb =  _context.Users.SingleOrDefault(user => user.Username == model.Username);
       
            if (userFromDb != null)
            {
                var passwordHash = _passwordHasher.HashPassword(userFromDb, model.Password);
                if (userFromDb.PasswordHash == passwordHash)
                {
                    LoggedUser = userFromDb;
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(nameof(model.Password), "Password not correct");
                return View(model);
            }
            ModelState.AddModelError(nameof(model.Username), "User not found");
            return View(model);
        }
    }
}
