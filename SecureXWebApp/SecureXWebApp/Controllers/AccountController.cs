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
    public class AccountController : AServiceController
    {
        public AccountController(HttpClient httpClient, Settings settings) : base(httpClient, settings)
        { }

        //GET: Account
        //ELA async
        public async Task<IActionResult> Index()
        {
            var uri = "Account";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(jsonString);
                if (TempData["CustomerId"] != null)
                {
                    var currentCustomerId = (int)TempData.Peek("CustomerId");
                    TempData.Keep("CustomerId");
                    accounts = accounts.FindAll(x => x.CustomerId == currentCustomerId);
                }
                if (accounts != null) return View(accounts);
                else return View();
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: Account/Details/5
        //ELA async
        public async Task<IActionResult> Details(Account Account)
        {
            var uri = $"Account/{Account.Id}";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                Account account = JsonConvert.DeserializeObject<Account>(jsonString);
                return View(account);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: Account/Create
        //ELA async
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        //ELA async
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account Account)
        {
            if (!ModelState.IsValid)
            {
                return View(Account);
            }
            try
            {
                Account.Status = "Pending";
                var currentCustomerId = (int)TempData.Peek("CustomerId");
                TempData.Keep("CustomerId");
                Account.CustomerId = currentCustomerId;
                var uri = $"Account";
                var request = CreateRequestToService(HttpMethod.Post, uri, Account);
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Account Account)
        {
            var uri = $"Account/{Account.Id}";
            var request = CreateRequestToService(HttpMethod.Put, uri, Account);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                var account = JsonConvert.DeserializeObject<Account>(jsonString);

                return View(account);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: Account/Freeze/5
        public async Task<IActionResult> Freeze(int id)
        {
            var uri = $"Account/{id}";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                Account account = JsonConvert.DeserializeObject<Account>(jsonString);
                return View(account);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // PUT: Account/Freeze/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Freeze(Account Account)
        {
            Account.Status = "Frozen";
            var uri = $"Account/{Account.Id}";
            var request = CreateRequestToService(HttpMethod.Put, uri, Account);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                var account = JsonConvert.DeserializeObject<Account>(jsonString);

                return View("Details", Account);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: Account/Approve/5
        public async Task<IActionResult> Approve(int id)
        {
            var uri = $"Account/{id}";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                Account account = JsonConvert.DeserializeObject<Account>(jsonString);
                return View(account);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // PUT: Account/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Account Account)
        {
            Account.Status = "Active";
            var uri = $"Account/{Account.Id}";
            var request = CreateRequestToService(HttpMethod.Put, uri, Account);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                var account = JsonConvert.DeserializeObject<Account>(jsonString);

                return View("Details", Account);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Account/Delete/5
        //ELA async
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Account account)
        {
            {
                var uri = $"Account/{account.Id}";
                var request = CreateRequestToService(HttpMethod.Delete, uri);
                try
                {
                    var response = await HttpClient.SendAsync(request);
                    if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);

                    return View("Index");
                }
                catch
                {
                    return View("Error", new ErrorViewModel());
                }
            }
        }
    }
}