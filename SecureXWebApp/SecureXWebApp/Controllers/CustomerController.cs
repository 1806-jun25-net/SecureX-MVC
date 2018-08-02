using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecureXWebApp.Models;

namespace SecureXWebApp.Controllers
{

    public class CustomerController : AServiceController
    {
        public CustomerController(HttpClient httpClient) : base(httpClient)
        { }

        //GET: Customer
        //ELA async
        public async Task<IActionResult> Index()
        {
            var uri = "Customer";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(jsonString);
                return View(customers);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Customer/Details/5
        //ELA async
        public async Task<IActionResult> Details(Customer Customer)
        {
            var uri = $"Customer/{Customer.Id}";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                Customer customer = JsonConvert.DeserializeObject<Customer>(jsonString);
                return View(customer);
            }
            catch
            {
                return View("Error");
            }
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
                var uri = $"Customer/{Customer.Id}";
                var request = CreateRequestToService(HttpMethod.Post, uri, Customer);
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) SelectErrorView(response);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error");
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
            var uri = $"Customer/{Customer.Id}";
            var request = CreateRequestToService(HttpMethod.Put, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(jsonString);

                return View(customer);
            }
            catch
            {
                return View("Error");
            }
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
                var uri = $"Customer/{Customer.Id}";
                var request = CreateRequestToService(HttpMethod.Delete, uri);
                try
                {
                    var response = await HttpClient.SendAsync(request);
                    if (CheckIfErrorStatusCode(response)) SelectErrorView(response);

                    return View("Index");
                }
                catch
                {
                    return View("Error");
                }
            }
        }
    }
}