using System;  
using System.Net.Http;  
using System.Net.Http.Headers;  
using MoviesMVC.Models;
  
namespace MoviesMVC.Services.Helper  
{  
    public class HttpClientAPI  
    {    
        public HttpClient InitializeClient(string _apiBaseURI)  
        {  
            //private string _userApiBaseURI = "http://localhost:5002";
            //private string _jwtApiBaseURI = "http://localhost:5003";

            var client = new HttpClient();  
            //Passing service base url    
            client.BaseAddress = new Uri(_apiBaseURI);  
  
            client.DefaultRequestHeaders.Clear();  
            //Define request data format    
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));  
  
            return client;  
        }  
    }
}  