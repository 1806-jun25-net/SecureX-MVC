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
    public class AccountController : AServiceController
    {
        public AccountController(HttpClient httpClient) : base(httpClient)
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
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error : Account/Index");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(jsonString);
                return View(accounts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View("Error : Account/Index");
        }

        // GET: Account/Details/5
        //ELA async
        public async Task<IActionResult> Details(Account account)
        {
            var uri = $"Account/{account.Id}";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : Account/{account.Id}");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                Account _account = JsonConvert.DeserializeObject<List<Account>>(jsonString).FirstOrDefault(x => x.Id == account.Id);
                return View(_account);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View($"Error : Account/{account.Id}");
        }

        // GET: Account/Create
        //ELA async
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                var uri = $"Account/{Account.Id}";
                var request = CreateRequestToService(HttpMethod.Post, uri, User);

                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : Account/{Account.Id}");
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View($"Error : Account/{Account.Id}");
            }
        }

        // GET: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Account Account)
        {
            var uri = $"Account/{Account.Id}";
            var request = CreateRequestToService(HttpMethod.Put, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : Account/{Account.Id}");
                }

                string jsonString = await response.Content.ReadAsStringAsync();
                var account = JsonConvert.DeserializeObject<List<Account>>(jsonString).FirstOrDefault(x => x.Id == Account.Id);

                return View(account);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return View($"Error in Account/{Account.Id}");
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
                    if (!response.IsSuccessStatusCode)
                    {
                        return View($"Error: Account/{account.Id}");
                    }

                    return View("Account was deleted.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return View($"Error in Account/{account.Id}");
            }
        }
    }
}