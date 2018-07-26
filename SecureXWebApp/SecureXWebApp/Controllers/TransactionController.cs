using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecureXWebApp.Models;

namespace SecureXWebApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly static string ServiceUri = "http://securex-api.azurewebsites.net/api/";

        public HttpClient HttpClient { get; }

        public TransactionController(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        //GET: Transaction
        //ELA async
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var uri = ServiceUri + "Transaction/Index";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error : Transaction/Index");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Transaction> transaction = JsonConvert.DeserializeObject<List<Transaction>>(jsonString);
                return View(transaction);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View("Error : Transaction/Index");
        }

        // GET: Transaction/Details/5
        //ELA async
        public async Task<IActionResult> Details(Transaction Transaction)
        {
            var uri = ServiceUri + $"Transaction/{Transaction.Id}";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : Transaction/{Transaction.Id}");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                Transaction transaction = JsonConvert.DeserializeObject<List<Transaction>>(jsonString).FirstOrDefault(x => x.Id == Transaction.Id);
                return View(transaction);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View($"Error : Transaction/{Transaction.Id}");
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
                string jsonString = JsonConvert.SerializeObject(Transaction);
                var uri = ServiceUri + $"Transaction/{Transaction.Id}";
                var request = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    // we set what the Content-Type header will be here
                    Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
                };

                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : Transaction/{Transaction.Id}");
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View($"Error : Transaction/{Transaction.Id}");
            }
        }