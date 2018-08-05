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
    public class UserController : AServiceController
    {
        public UserController(HttpClient httpClient) : base(httpClient)
        { }

        //GET: User
        //ELA async
        public async Task<IActionResult> Index()
        {
            var uri = "User";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonString);
                return View(users);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: User/Customer Profile
        public async Task<IActionResult> Profile()
        {
            try
            {
                var currentUserName = (string)TempData.Peek("UserName");
                TempData.Keep("UserName");                                
                var uri = "User";
                var request = CreateRequestToService(HttpMethod.Get, uri);
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonString);                
                var user = users.First(x => x.UserName == currentUserName);           
                                
                try
                {
                    var currentCustomerId = (int)TempData.Peek("CustomerId");
                    TempData.Keep("CustomerId");
                    uri = "Customer";
                    request = CreateRequestToService(HttpMethod.Get, uri);
                    response = await HttpClient.SendAsync(request);
                    if (CheckIfErrorStatusCode(response)) SelectErrorView(response);
                    jsonString = await response.Content.ReadAsStringAsync();
                    List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(jsonString);
                    var customer = customers.First(x => x.Id == currentCustomerId);

                    var profileVM = new ProfileViewModel
                    {
                        User = user,
                        Customer = customer
                    };

                    return View(profileVM);
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Failed to load cutomer info."));
                }
            }
            catch
            {
                return View("Error", new ErrorViewModel("Failed to load user info."));
            }
        }

        // GET: User/Details/5
        //ELA async
        public async Task<IActionResult> Details(User User)
        {
            var uri = $"User/{User.Id}";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                User user = JsonConvert.DeserializeObject<User>(jsonString);
                return View(user);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

         //Combined User Create into Login/Register
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
                var uri = "User";
                var request = CreateRequestToService(HttpMethod.Post, uri, User);
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) SelectErrorView(response);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new ErrorViewModel());
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
            var uri = $"User/{User.Id}";
            var request = CreateRequestToService(HttpMethod.Put, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) SelectErrorView(response);

                string jsonString = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(jsonString);

                return View(user);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
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
                var uri = $"User/{User.Id}";
                var request = CreateRequestToService(HttpMethod.Delete, uri);
                try
                {
                    var response = await HttpClient.SendAsync(request);
                    if (CheckIfErrorStatusCode(response)) SelectErrorView(response);

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