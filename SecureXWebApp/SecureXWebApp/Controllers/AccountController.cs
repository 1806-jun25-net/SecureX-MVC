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
    public class AccountController : Controller
    {
        private readonly static string ServiceUri = "http://securex-api.azurewebsites.net/api/";

        public HttpClient HttpClient { get; }

        public AccountController(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        //GET: Account
        //ELA async
        public async Task<IActionResult> Index()
        {
            var uri = ServiceUri + "Account";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
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
            var uri = ServiceUri + $"Account/{account.Id}";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
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
                string jsonString = JsonConvert.SerializeObject(User);
                var uri = ServiceUri + $"Account/{Account.Id}";
                var request = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    // we set what the Content-Type header will be here
                    Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
                };

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
        public async Task<IActionResult> Edit(User User)
        {
            var uri = ServiceUri + $"User/{User.Id}";
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : User/{User.Id}");
                }

                string jsonString = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<List<User>>(jsonString).FirstOrDefault(x => x.Id == User.Id);

                return View(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return View($"Error in User/{User.Id}");
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
                var uri = ServiceUri + $"Account/{account.Id}";
                var request = new HttpRequestMessage(HttpMethod.Delete, uri);
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