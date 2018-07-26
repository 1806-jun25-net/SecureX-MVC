using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecureXWebApp.Controllers
{
    public class BankController : Controller
    {
        private readonly static string ServiceUri = "http://securex-api.azurewebsites.net/api/";

        public HttpClient HttpClient { get; }

        public BankController(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        // GET: Bank
        public ActionResult Index()
        {
            return View();
        }

        // GET: Bank/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        
    }
}