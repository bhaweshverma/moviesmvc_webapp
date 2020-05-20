using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviesMVC.Models;
using MoviesMVC.ViewModels;
using MoviesMVC.Services.Helper;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace MoviesMVC.Controllers
{
    public class AccountController : Controller
    {
        //private readonly UserManager<User> userManager;
        //private readonly SignInManager<User> signInManager;
        private HttpClientAPI httpClient = new HttpClientAPI();
        private string _jwtApiBaseUri = "http://localhost:5003";
        private IConfiguration _config;
        private readonly ILogger _logger; 

        public AccountController(IConfiguration config, ILogger<AccountController> logger)
        {
            this._config = config;
            this._logger = logger;
        }    
        /*public AccountController(UserManager<User> userManager, 
        SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        */
        [HttpGet]
        public IActionResult Register()
        {
            var message = $"Register page opened at {DateTime.Now}";
            _logger.LogInformation(message);
            _logger.LogWarning(message);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new User{
                    UserName = model.Email,
                    Email = model.Email,
                    Password = model.Password
                };
               
                HttpClient client = httpClient.InitializeClient(_jwtApiBaseUri);
                client.DefaultRequestHeaders.Add("JWTAppKey",_config["JWTApp:Key"]);
                client.DefaultRequestHeaders.Add("JWTAppSecret", _config["JWTApp:Secret"]);

                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/authorize/register",content);

                var result = await response.Content.ReadAsStringAsync();
                string error = string.Empty;

                if(response.IsSuccessStatusCode)
                {
                   //await signInManager.SignInAsync(user, isPersistent:false);
                   return RedirectToAction("login","account");
                   //return Content("User successfully registered!");
                }
                else
                {
                    if(!string.IsNullOrEmpty(result))
                    error = JsonConvert.DeserializeObject(result).ToString();
                    else
                    error = response.ReasonPhrase;
                }
                
                ModelState.AddModelError(string.Empty,"Registration failed "+ error);
            }
            return View(model);
        }
   
        [HttpGet]
        public IActionResult Login(){
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                //var result1 = new IdentityUser {Email = model.Email};
                //var result = await signInManager.PasswordSignInAsync(model.Email, model.Password,  model.RememberMe, lockoutOnFailure:false);
                
                HttpClient client = httpClient.InitializeClient(_jwtApiBaseUri);

                var content = new StringContent(JsonConvert.SerializeObject(model),Encoding.UTF8,"application/json");

                HttpResponseMessage response = await client.PostAsync("api/authorize/token", content);

                var result = await response.Content.ReadAsStringAsync();
                string error = string.Empty;

                if(response.IsSuccessStatusCode)
                {
                    //return RedirectToAction("index", "users");
                    error = JsonConvert.DeserializeObject(result).ToString();
                    /*
                    var user = HttpContext.User;
                    string email = string.Empty;
                    if(user.HasClaim(c => c.Type == "Email"))
                    {
                        foreach(var claim in user.Claims)
                        {
                            if(claim.Type == "Email")
                            {
                                email = claim.Value;
                            }
                        }
                        //var email = user.Claims.FirstOrDefault(c => c.Type == "Email").Value;
                    }*/
                    return Content(string.Format("You {0} have logged in successfully. {1}", model.Email, error)); 
                }
                else
                {
                    if(!string.IsNullOrEmpty(result))
                    error = JsonConvert.DeserializeObject(result).ToString();
                    else
                    error = response.ReasonPhrase;
                    
                    //return Content(error);
                }
                ModelState.AddModelError(string.Empty, string.Format("Invalid Login Attempt. Description : {0} ", error));
            }
            return View();
        }
    }
}