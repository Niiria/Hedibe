using Hedibe.Models;
using Hedibe.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hedibe.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserContextService _userContextService;
        public HomeController(IUserContextService userContextService)
        {
            _userContextService = userContextService;
        }

        public IActionResult Index()
        {
            var userRole = _userContextService.GetRole();
            if (userRole is not null)
            switch (userRole)
                {
                    case "User":
                        return RedirectToAction("Landing", "Home");
                    case "Moderator":
                        return RedirectToAction("Dashboard", "Panel");
                    case "Admin":
                        return RedirectToAction("Dashboard", "Panel");
                    default:
                        break;
                }
            return View();
        }

        public IActionResult Landing()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
