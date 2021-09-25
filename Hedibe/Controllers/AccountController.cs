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
using Hedibe.Services;
using Hedibe.Models.Account;

namespace Hedibe.Controllers
{
    public class AccountController : Controller
    {
        private readonly HedibeDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserContextService _userContextService;

        public AccountController(HedibeDbContext context, IPasswordHasher<User> passwordHasher, IUserContextService userContextService )
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _userContextService = userContextService;
        }


        // GET: Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // GET: Auth/Login
        [HttpGet]
        public IActionResult Login(LoginUserDto model)
        {
            ModelState.Clear();
            return View(model);
        }

        // GET: Auth/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            _userContextService.LogoutUser();
            return RedirectToAction("Index", "Home");
        }


        // POST: Auth/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto model)
        {
            if (ModelState.IsValid)
            {
                User UserToDb = new() { Username = model.Username, Email = model.Email, RoleId=3 };
                var passwordHash = _passwordHasher.HashPassword(UserToDb, model.Password);
                UserToDb.PasswordHash = passwordHash;

                _context.Users.Add(UserToDb);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", model);
            }
            return View(model);
          }


        // POST: Auth/Login
        [HttpPost]
        [ActionName("Login")]
        public IActionResult Login_submit(LoginUserDto model)
        {
            var userFromDb =  _context.Users.SingleOrDefault(user => user.Username == model.Username);
       
            if (userFromDb is null)
            {
                ModelState.AddModelError(nameof(model.Username), "User not found");
                return View(model);
            }
            
            var verificationResult = _passwordHasher.VerifyHashedPassword(userFromDb, userFromDb.PasswordHash, model.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(nameof(model.Password), "Password not correct");
                return View(model);
            }
                    
            var loginUser = new LoggedUser() { Username = userFromDb.Username, Email = userFromDb.Email, Role = userFromDb.Role };
            _userContextService.LoginUser(loginUser);
            return RedirectToAction("Index", "Home");
               
        }
    }
}
