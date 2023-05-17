using AspNetCoreHero.ToastNotification.Abstractions;
using FirstTask.Entities.Models;
using FirstTask.Entities.ViewModels;
using FirstTask.Repository.Interface;
using FirstTaskWeb.AuthHelper;
using FirstTaskWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FirstTaskWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _notify;
        private readonly IUserRepository _userRepository;
        private readonly IListRepository _listRepository;
        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, INotyfService notify, IUserRepository userRepository, IListRepository listRepository)
        {
            _configuration = configuration; 
            _logger = logger;
            _notify = notify;
            _userRepository = userRepository;
            _listRepository = listRepository;
        }
        public IActionResult Index(User model)
        {
            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Register()
        {
            var getCountryList = _listRepository.GetCountryListAsync();
            ViewBag.Countrys = await getCountryList;
            RegisterVM model = new RegisterVM();
            model.CountryList = await getCountryList;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var FindUser = await _userRepository.GetFirstOrDefaultAsync(user => user.Email == model.Email);
                    if (FindUser != null)
                    {
                        _notify.Error("This Email is Already Registered. Please Login!");
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        var GetNewUser = await _userRepository.RegisterAsync(model);
                        if (GetNewUser == true)
                        {
                            await _userRepository.SaveDbAsync();
                            _notify.Success("Registeration Successful. Please Login!");
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            _notify.Error("Registration Failed. Try Again Later!");
                        }
                    }
                }
            }
            catch
            {
                _notify.Error("Registration Service is currently unavailable. Try Again Later!");
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var FindUser = await _userRepository.GetFirstOrDefaultAsync(user => user.Email == model.Email.ToLower());
                    if (FindUser != null)
                    {
                        var CheckPassword = BCrypt.Net.BCrypt.Verify(model.Password.ToLower(), FindUser.Password);
                        if (CheckPassword == true)
                        {
                            var jwtSettings = _configuration.GetSection(nameof(JWTSettings)).Get<JWTSettings>();
                            var token = JwtTokenHelper.GenerateToken(jwtSettings, FindUser);
                            HttpContext.Session.SetString("Token",token);
                            _notify.Success($"Welcome User:{FindUser.FirstName + " " + FindUser.LastName }", durationInSeconds: 5);
                            return RedirectToAction("Index", FindUser);
                        }
                        else
                        {
                            _notify.Error("Invalid Credentials!");
                            return View(model);
                        }
                    }
                    else
                    {
                        TempData["success"] = "Please Register";
                        return RedirectToAction("Register");
                    }
                }
            }
            catch
            {
                TempData["error"] = "Try Again Later!";
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            return RedirectToAction("Index");
        }
        public async Task<JsonResult> GetStateListByCountryIdJson([FromQuery] long countryId)
        {
            var getList = await _listRepository.GetStateListByCountryIdAsync(countryId);
            return Json(getList);
        }

        public async Task<JsonResult> GetCityListByStateIdJson([FromQuery] long stateId)
        {
            var getList = await _listRepository.GetCityListByStateIdAsync(stateId);
            return Json(getList);
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
    }
}