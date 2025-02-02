using System.Diagnostics;
using IdentityVersion2.Entities;
using IdentityVersion2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace IdentityVersion2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> singInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _singInManager = singInManager;
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
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    Email = model.Email,
                    Gender = model.Gender,
                    UserName = model.UserName,
                };
                var identityResult= await _userManager.CreateAsync(appUser, password: model.Password);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }

               
            }

            return View(model);
        }

        public IActionResult SingIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SingIn(UserSingInModel model)
        {
            if (ModelState.IsValid)
            {
               var singInResult= await _singInManager.PasswordSignInAsync(model.UserName, model.Password, false, true);
               if (singInResult.Succeeded)
               {
                   // bu iş başarılıdır
               }
               else if (singInResult.IsLockedOut)
               {
                   //Hesap kilitli
               }
               else if (singInResult.IsNotAllowed)
               {
                   //email && phone number doğrulanmış
               }
            }

            return View(model);
        }
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var userName = User.Identity.Name;
            return View();
        }
    }
}
