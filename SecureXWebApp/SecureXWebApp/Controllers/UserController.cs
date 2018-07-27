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
    public class UserController : Controller
    {
        private readonly static string ServiceUri = "http://securex-api.azurewebsites.net/api/";
        public HttpClient HttpClient { get; }
        public UserController(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        //GET: User
        //ELA async
        public async Task<IActionResult> Index()
        {
            var uri = ServiceUri + "User";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error : User/Index");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonString);
                return View(users);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View("Error : User/Index");
        }

        // GET: User/Details/5
        //ELA async
        public async Task<IActionResult> Details(User User)
        {
            var uri = ServiceUri + $"User/{User.Id}";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : User/{User.Id}");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                User user = JsonConvert.DeserializeObject<List<User>>(jsonString).FirstOrDefault(x => x.Id == User.Id);
                return View(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View($"Error : User/{User.Id}");
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        //ELA async
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User User)
        {
            if (!ModelState.IsValid)
            {
                return View(User);
            }
            try
            {
                string jsonString = JsonConvert.SerializeObject(User);
                var uri = ServiceUri + $"User/{User.Id}";
                var request = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    // we set what the Content-Type header will be here
                    Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
                };

                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : User/{User.Id}");
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View($"Error : User/{User.Id}");
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit()
        {
            return View();
        }

        // POST: User/Edit/5
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

        // GET: User/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        // POST: User/Delete/5
        //ELA async
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(User User)
        {
            {
                var uri = ServiceUri + $"User/{User.Id}";
                var request = new HttpRequestMessage(HttpMethod.Delete, uri);
                try
                {
                    var response = await HttpClient.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        return View($"Error: User/{User.Id}");
                    }
                   
                    return View("User was deleted.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return View($"Error in User/{User.Id}");
            }
        }
    }
}