using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecureXWebApp.Models;

namespace SecureXWebApp.Controllers
{
    public class EmployeeController : AServiceController
    {
        public EmployeeController(HttpClient httpClient) : base(httpClient)
        { }

        // GET: Employee
        //ELA async
        public async Task<IActionResult> Index()
        {
            var uri = "Employee";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error : Employee/Index");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Employee> employee = JsonConvert.DeserializeObject<List<Employee>>(jsonString);
                return View(employee);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View("Error : Employee/Index");
        }

        // GET: Employee/Details/5
        //ELA async
        public async Task<IActionResult> Details(Employee Employee)
        {
            var uri = $"Employee/{Employee.Id}";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : Employee/{Employee.Id}");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                Employee employee = JsonConvert.DeserializeObject<List<Employee>>(jsonString).FirstOrDefault(x => x.Id == Employee.Id);
                return View(employee);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View($"Error : Employee/{Employee.Id}");
        }

    }
}