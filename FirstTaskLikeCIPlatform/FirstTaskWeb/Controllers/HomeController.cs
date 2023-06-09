﻿using AspNetCoreHero.ToastNotification.Abstractions;
using FirstTask.Entities.Models;
using FirstTask.Entities.ViewModels;
using FirstTask.Repository.Interface;
using FirstTaskWeb.AuthHelper;
using FirstTaskWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace FirstTaskWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotyfService _notify;
        private readonly IUserRepository _userRepository;
        private readonly IListRepository _listRepository;

        public HomeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<HomeController> logger, INotyfService notify, IUserRepository userRepository, IListRepository listRepository)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _notify = notify;
            _userRepository = userRepository;
            _listRepository = listRepository;
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated) //true
            {
                //var getUserDetails = await _userRepository.GetFirstOrDefaultAsync(x => x.UserId == Convert.ToInt32(User.Identity.Name));
                User getUserDetails = new() { UserId = 20, FirstName = "Smit", LastName = "test" };
                return View(getUserDetails);
            }
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return RedirectToAction("Login");
        }
        public IActionResult ForbiddenResult()
        {
            return RedirectToAction("Login");
        }
        #region Login Logic
        /// <summary>
        /// Login View Method
        /// Get Mehtod of Login: 
        /// Check if user is authenticated then user can see Home("Index") Page
        /// Else View Login Page(View)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string? emailId)
        {
            if (!string.IsNullOrEmpty(emailId))
            {
                //Populated EmailId in model
                LoginVM model = new();
                model.Email = emailId;
                return View(model);
            }
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            /*var token = HttpContext.Request.Cookies["JWTToken"]?.ToString();
            if (token != null)
            {
                var principles = JwtTokenHelper.ValidateToken(token);
                if (principles != null)
                {
                    if (principles.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                return View();
            }*/

            return View();

        }
        /// <summary>
        /// Post Method of Login Check Credentials 
        /// If all correct then Redirect to Home Page
        /// Logic: Check 
        /// 1. If Email is present in DB if present then check password is correct respective to Email
        /// => If password is not correct return message invalid credentials Else Redirect to Home Page with Message Welcome user: Username
        /// 2. If Email is not present in DB then Redirect To Register Page with Email Field Populated
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Check if User exists with this Email
                    /* var RegisteredUser = await _userRepository.GetFirstOrDefaultAsync(user => user.Email == loginModel.Email.ToLower());
                     if (RegisteredUser != null) //User exists => True
                     {
                    var CheckPassword = BCrypt.Net.BCrypt.Verify(loginModel.Password.Trim().ToLower(), RegisteredUser.Password);
                    */
                    User RegisteredUser = new() { UserId =20, FirstName = "Smit", LastName ="D",CityId = 1, CountryId = 1, GenderId = 1 ,RoleId = 1 , DeletedAt = null };
                    var CheckPassword = true;
                        if (CheckPassword == true) //Correct password
                        {
                            var jwtSettings = _configuration.GetSection(nameof(JWTSettings)).Get<JWTSettings>(); //get JetToken settings values from configuration
                            var token = JwtTokenHelper.GenerateToken(jwtSettings, RegisteredUser); //generate JwtToken
                            //Add Token to cookie
                            if (token != null)
                            {
                            //HttpContext.Response.Headers.Authorization.Append(token);
                            /* var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7284/Home/Index");
                             request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);*/
                            //HttpContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token); 
                            /*                                HttpClient client = new HttpClient();
                                                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString()); */
                            HttpContext.Request.Headers.Add("Authorization", "Bearer " + token);
                            HttpContext.Response.Cookies.Append("JWTToken", token, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None, Expires = DateTime.Now.AddMinutes(5) });
                                
                            _notify.Success($"Welcome User:{RegisteredUser.FirstName + " " + RegisteredUser.LastName }", durationInSeconds: 5);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return RedirectToAction("Login");
                            }
                        }
                        else //Password Wrong
                        {
                            _notify.Error("Invalid Credentials!", durationInSeconds: 3);
                            return View(loginModel);
                        }
                    /*}
                    else //User exists => False
                    {
                        _notify.Information("Email Id is not Registered. Please Register to Login Credentials!", durationInSeconds: 5);
                        return RedirectToAction("Register", new { emailId = loginModel.Email });
                    }*/
                }
                else
                {
                    //ModeState invalid or 
                    return View(loginModel);
                }
            }
            catch
            {
                //Exception occurs
                _notify.Error("Try Again Later!", durationInSeconds: 3);
                return View(loginModel);
            }

        }
        #endregion

        #region Register Logic
        /// <summary>
        /// Register Page View
        /// Get Method of Registration
        /// Check if User is redirected from login with Email Id then
        /// populate email id in model Else add CountryList into model
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <returns></returns>
        public async Task<IActionResult> Register(string? emailId)
        {
            var getCountryList = _listRepository.GetCountryListAsync();
            RegisterVM model = new RegisterVM();
            if (!string.IsNullOrEmpty(emailId))
            {
                model.Email = emailId;
            }
            model.CountryList = await getCountryList;
            return View(model);
        }
        /// <summary>
        /// Post Method of Registration
        /// Check 
        /// 1. If Email already Registered 
        ///  if true RedirectoLogin with Email populated
        /// 2. If Email is not Registered then Register new User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var RegisteredUser = await _userRepository.GetFirstOrDefaultAsync(user => user.Email == registerModel.Email);
                    //If Email is already registered(i.iFindUser not null) then redirect user to login with msg
                    if (RegisteredUser != null)
                    {
                        _notify.Information("Email Already Registered. Please Login!");
                        return RedirectToAction("Login", new { emailId = registerModel.Email });
                    }
                    else
                    {
                        //Method Registers new User if successfully Registered returns true else false
                        var GetNewUser = await _userRepository.RegisterAsync(registerModel);
                        if (GetNewUser == true)
                        {
                            await _userRepository.SaveDbAsync();
                            _notify.Success("Registeration Successful. Please Login!");
                            return RedirectToAction("Login", new { emailId = registerModel.Email });
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
            registerModel.CountryList = await _listRepository.GetCountryListAsync();
            return View(registerModel);
        }

        #endregion

        #region Logout
        //Clear Cookiee i.e. remove JtToken
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("JWTToken");
            return RedirectToAction("Login");
        }
        #endregion

        public IActionResult UserNotFound()
        {
            _notify.Error("UNF,Login First");
            return RedirectToAction("Login");
        }
        public IActionResult UnAuthorized()
        {
            _notify.Error("UNAuth,Login First");
            return RedirectToAction("Login");
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