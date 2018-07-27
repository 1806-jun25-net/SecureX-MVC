using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecureXWebApp.Models;

namespace SecureXWebApp.Controllers
{

    public class CustomerController : Controller
    {
        private readonly static string ServiceUri = "http://securex-api.azurewebsites.net/api/";

        public HttpClient HttpClient { get; }

        public CustomerController(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        //GET: Customer
        //ELA async
        public async Task<IActionResult> Index()
        {
            var uri = ServiceUri + "Customer";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error : Customer/Index");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(jsonString);
                return View(customers);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View("Error : Customer/Index");
        }

        // GET: Customer/Details/5
        //ELA async
        public async Task<IActionResult> Details(Customer Customer)
        {
            var uri = ServiceUri + $"Customer/{Customer.Id}";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : Customer/{Customer.Id}");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                Customer customer = JsonConvert.DeserializeObject<List<Customer>>(jsonString).FirstOrDefault(x => x.Id == Customer.Id);
                return View(customer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View($"Error : Customer/{Customer.Id}");
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        //ELA async
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer Customer)
        {
            if (!ModelState.IsValid)
            {
                return View(Customer);
            }
            try
            {
                string jsonString = JsonConvert.SerializeObject(Customer);
                var uri = ServiceUri + $"Customer/{Customer.Id}";
                var request = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    // we set what the Content-Type header will be here
                    Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
                };

                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : Customer/{Customer.Id}");
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View($"Error : Customer/{Customer.Id}");
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer Customer)
        {
            var uri = ServiceUri + $"Customer/{Customer.Id}";
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : Customer/{Customer.Id}");
                }

                string jsonString = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<List<Customer>>(jsonString).FirstOrDefault(x => x.Id == Customer.Id);

                return View(customer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return View($"Error in Customer/{Customer.Id}");
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customer/Delete/5
        //ELA async
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Customer Customer)
        {
            {
                var uri = ServiceUri + $"Customer/{Customer.Id}";
                var request = new HttpRequestMessage(HttpMethod.Delete, uri);
                try
                {
                    var response = await HttpClient.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        return View($"Error: Customer/{Customer.Id}");
                    }

                    return View("Customer was deleted.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return View($"Error in Customer/{Customer.Id}");
            }
        }
    }
}