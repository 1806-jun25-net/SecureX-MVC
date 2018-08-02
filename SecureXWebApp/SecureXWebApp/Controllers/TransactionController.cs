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
    public class TransactionController : AServiceController
    {
        public TransactionController(HttpClient httpClient) : base(httpClient)
        { }

        //GET: Transaction
        //ELA async
        public async Task<IActionResult> Index()
        {
            var uri = "Transaction";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Transaction> transaction = JsonConvert.DeserializeObject<List<Transaction>>(jsonString);
                return View(transaction);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Transaction/Details/5
        //ELA async
        public async Task<IActionResult> Details(Transaction Transaction)
        {
            var uri = $"Transaction/{Transaction.Id}";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                Transaction transaction = JsonConvert.DeserializeObject<Transaction>(jsonString);
                return View(transaction);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Transaction/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transaction/Create
        // ELA async
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction Transaction)
        {
            if (!ModelState.IsValid)
            {
                return View(Transaction);
            }
            try
            {
                var uri = $"Transaction/{Transaction.Id}";
                var request = CreateRequestToService(HttpMethod.Post, uri, Transaction);
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) SelectErrorView(response);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error");
            }
        }
    }
}