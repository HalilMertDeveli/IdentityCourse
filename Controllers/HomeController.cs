using System.Diagnostics;
using IdentityVersion2.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityVersion2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Guid tryGuid = Guid.NewGuid();
            TempData["guidThing"] = tryGuid;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Create()
        {
            return View(new UserCreateModel());
        }
        [HttpPost]
        public IActionResult Create(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                //business
            }

            return View(model);
        }
    }
}
