using Microsoft.AspNetCore.Mvc;  
using Newtonsoft.Json;  
using MoviesMVC.Services.Helper;  
using System.Collections.Generic;  
using System.Linq;  
using System.Net.Http;  
using System.Text;  
using System.Threading.Tasks;  
using MoviesMVC.Models;
  
namespace MoviesMVC.Controllers  
{  
    public class UsersController : Controller  
    {  
        HttpClientAPI _userAPI = new HttpClientAPI();  
  
        public async Task<IActionResult> Index()  
        {  
            List<User> dto = new List<User>();  
  
            HttpClient client = _userAPI.InitializeClient("http://localhost:5002");  
  
            HttpResponseMessage res = await client.GetAsync("api/v1/users");  
  
            //Checking the response is successful or not which is sent using HttpClient    
            if (res.IsSuccessStatusCode)  
            {  
                //Storing the response details recieved from web api     
                var result = res.Content.ReadAsStringAsync().Result;  
  
                //Deserializing the response recieved from web api and storing into the Employee list    
                dto = JsonConvert.DeserializeObject<List<User>>(result);  

            }  
            //returning the employee list to view    
            return View(dto);  
        }  
  
        // GET: User/Create  
        public IActionResult Create()  
        {  
            return View();  
        }  
  
        // POST: User/Create  
        [HttpPost]  
        public IActionResult Create([Bind("UserId,UserName,UserType")] User user)  
        {  
            if (ModelState.IsValid)  
            {  
                HttpClient client = _userAPI.InitializeClient("http://localhost:5002");  
  
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8,"application/json");  
                HttpResponseMessage res = client.PostAsync("api/v1/users", content).Result;  
                if (res.IsSuccessStatusCode)  
                {  
                    return RedirectToAction("Index");  
                }  
            }  
            return View(user);  
        }  
    }     
}  