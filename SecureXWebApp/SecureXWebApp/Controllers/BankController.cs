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
    public class BankController : AServiceController
    {
        public BankController(HttpClient httpClient) : base(httpClient)
        { }

        // GET: Bank
        //ELA async        
        public async Task<IActionResult> Index()
        {
            var uri = "Bank";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error : Bank/Index");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Bank> banks = JsonConvert.DeserializeObject<List<Bank>>(jsonString);
                return View(banks);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View("Error : Bank/Index");
        }

        //GET: User/Details/5
        //ELA async
        public async Task<IActionResult> Details(Bank Bank)
        {
            var uri = $"Bank/{Bank.Id}";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : Bank/{Bank.Id}");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                Bank bank = JsonConvert.DeserializeObject<Bank>(jsonString);
                return View(bank);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View($"Error: Bank/{Bank.Id}");
        }

    }
}