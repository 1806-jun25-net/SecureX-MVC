using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecureXWebApp.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        public HttpClient HttpClient { get; }

        public EmployeeController(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

    }
}