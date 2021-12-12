using Hedibe.Entities;
using Hedibe.Models.Account;
using Hedibe.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Controllers
{
    public class AccountController : Controller
    {
        private readonly HedibeDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserContextService _userContextService;

        public AccountController(HedibeDbContext context, IPasswordHasher<User> passwordHasher, IUserContextService userContextService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _userContextService = userContextService;
        }


        // GET: Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

            return View();
        }

        // GET: Auth/Login
        [HttpGet]
        public IActionResult Login(LoginUserDto model)
        {
            if (TempData["AddInfo"] is not null)
                ViewBag.AddInfo = TempData["AddInfo"];
            else
                ViewBag.AddInfo = null;

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
                User UserToDb = new() { Username = model.Username, Email = model.Email, RoleId = 3 };
                var passwordHash = _passwordHasher.HashPassword(UserToDb, model.Password);
                UserToDb.PasswordHash = passwordHash;

                _context.Users.Add(UserToDb);
                await _context.SaveChangesAsync();
                if (_userContextService.CheckLoggedUser())
                {
                    if (_userContextService.GetLoggedUserRole() == "admin")
                        return RedirectToAction("Dashboard", "Panel");

                    if (_userContextService.GetLoggedUserRole() == "user")
                        return RedirectToAction("Index", "Products");
                }
                TempData["AddInfo"] = "Succesfully registered user!";
                return RedirectToAction("Login", model);
            }

            TempData["AddInfo"] = "Failed to register user!";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(RegisterUserDto model, string redirect, int page = 1)
        {
            User userFromDb = _context.Users.Include(r => r.Role).SingleOrDefault(user => user.Id == model.Id);
            if (userFromDb is null)
            {
                TempData["AddInfo"] = "Failed to edit user!";
                return RedirectToAction("EditUsers", "Panel", new { @page = page });
            }

            userFromDb.Username = model.Username;
            userFromDb.Email = model.Email;
            userFromDb.RoleId = model.RoleId;

            if (model.Password == model.ConfirmPassword)
            {
                User UserToDb = new() { Username = model.Username, Email = model.Email, RoleId = model.RoleId };
                var passwordHash = _passwordHasher.HashPassword(UserToDb, model.Password);
                userFromDb.PasswordHash = passwordHash;
            }

            _context.Users.Update(userFromDb);
            await _context.SaveChangesAsync();

            TempData["AddInfo"] = "Succesfully edited user!";
            return RedirectToAction("EditUsers", "Panel", new { @page = page });
        }


        // POST: Auth/Login
        [HttpPost]
        [ActionName("Login")]
        public IActionResult Login_submit(LoginUserDto model)
        {
            var userFromDb = _context.Users.Include(r => r.Role).SingleOrDefault(user => user.Username == model.Username);

            if (userFromDb is null)
            {
                ModelState.AddModelError(nameof(model.Username), "User not found");
                return View(model);
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(userFromDb, userFromDb.PasswordHash, model.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(nameof(model.Password), "Invalid password");
                return View(model);
            }

            var loginUser = new LoggedUser()
            {
                Id = userFromDb.Id,
                Username = userFromDb.Username,
                Email = userFromDb.Email,
                Role = userFromDb.Role
            };
            _userContextService.LoginUser(loginUser);

            if (loginUser.Role.Name == "User")
                return RedirectToAction("Index", "Products");

            if (loginUser.Role.Name == "Admin" || loginUser.Role.Name == "Mod")
                return RedirectToAction("Dashboard", "Panel");

            return View(model);
        }
    }
}
