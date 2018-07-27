using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecureXWebApp.Models;

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
        //ELA async
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var uri = ServiceUri + "Bank";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
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
            var uri = ServiceUri + $"Bank/{Bank.Id}";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : Bank/{Bank.Id}");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                Bank bank = JsonConvert.DeserializeObject<List<Bank>>(jsonString).FirstOrDefault(x => x.Id == Bank.Id);
                return View(bank);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View($"Error : Bank/{Bank.Id}");
        }

    }
}