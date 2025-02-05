using System.Diagnostics;
using System.Security.Claims;
using IdentityVersion2.Entities;
using IdentityVersion2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace IdentityVersion2.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singInManager;
        private readonly RoleManager<AppRole> _roleManager;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> singInManager, RoleManager<AppRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _singInManager = singInManager;
            _roleManager = roleManager;
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
                AppUser user = new AppUser()
                {
                    Email = model.Email,
                    Gender = model.Gender,
                    UserName = model.UserName,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                };

                var accurancyStamp = user.ConcurrencyStamp;

                var identityResult = await _userManager.CreateAsync(user, password: model.Password);
                if (identityResult.Succeeded)
                {
                    var memberRole = await _roleManager.FindByNameAsync("Member");
                    if (memberRole == null)
                    {
                        await _roleManager.CreateAsync(new()
                        {
                            Name = "Member",
                            CreatedTime = DateTime.Now,
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        });
                    }
                  
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Index");
                }

                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }


            }

            return View(model);
        }

        public IActionResult SingIn(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;

            return View(new UserSingInModel()
            {
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        public async Task<IActionResult> SingIn(UserSingInModel model)
        {
            if (ModelState.IsValid)
            {
                var singInResult = await _singInManager.PasswordSignInAsync(model.UserName, model.Password, false, true);

                if (string.IsNullOrWhiteSpace(model.ReturnUrl))
                {
                    Redirect(model.ReturnUrl);
                }

                if (singInResult.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Member"))
                    {
                        return RedirectToAction("MemberPanel");
                    }

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel");
                    }

                    return RedirectToAction("MemberPanel");


                }






            }

            return View(model);
        }
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var userName = User.Identity.Name;
            var role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            var stringRole = User.IsInRole("Admin");
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }
        [Authorize(Roles = "Member")]
        public IActionResult MemberPanel()
        {
            return View();
        }

        [Authorize(Roles = "Member")]
        public IActionResult MemberPage()
        {
            return View();
        }
    }
}
