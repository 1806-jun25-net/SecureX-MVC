﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecureXWebApp.Models;

namespace SecureXWebApp.Controllers
{

    public class LoginController : AServiceController
    {
        // how do we know what username and what roles we have for subsequent requests?
        // could make every API model include that info, so we know. (e.g. model base class)

        // could make an API endpoint to just tell me that info, and then I call that endpoint
        // every time I need to know who I am in MVC, and put that info into e.g. ViewData.
        // that it would be a good candidate for a custom action filter.

        // especially since if i want to put something dynamic in the layout / navbar for my user,
        // then that would be every request.

        public LoginController(HttpClient httpClient) : base(httpClient)
        { }

        // GET: User/Register
        public ViewResult Register()
        {
            return View();
        }

        //POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel register)
        {
            try
            {
                // post login/register
                var login = register.Login;
                var uri = "Login/Register";
                HttpRequestMessage request = CreateRequestToService(HttpMethod.Post, uri, login);
                HttpResponseMessage response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);

                // post login/login
                uri = "Login/Login";
                request = CreateRequestToService(HttpMethod.Post, uri, login);
                response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);

                PassCookiesToClient(response);

                // post customer
                var customer = register.Customer;
                customer.UserName = register.Login.UserName;
                uri = "Customer";
                request = CreateRequestToService(HttpMethod.Post, uri, customer);
                response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);

                // get customer id
                request = CreateRequestToService(HttpMethod.Get, uri);
                response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(jsonString);
                var customerId = customers.First(x => x.UserName == login.UserName).Id;

                // post user
                var user = register.User;
                user.UserName = register.Login.UserName;
                user.CustomerId = customerId;
                uri = "User";
                request = CreateRequestToService(HttpMethod.Post, uri, user);
                response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);

                // tempdata
                TempData["UserName"] = login.UserName;
                TempData["CustomerId"] = customerId;

            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Login/Login
        public ViewResult Login()
        {
            return View();
        }

        // POST: Login/Login
        [HttpPost]
        public async Task<ActionResult> Login(Login login)
        {
            HttpRequestMessage request = CreateRequestToService(HttpMethod.Post, "Login/Login", login);

            HttpResponseMessage response;
            try
            {
                response = await HttpClient.SendAsync(request);
            }
            catch (AggregateException)
            {
                return View("Error", new ErrorViewModel());
            }

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewData["ErrorMessage"] = "Login credentials are invalid. Please try again or register if new.";
                    return View();
                }
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
            }

            PassCookiesToClient(response);

            // get customer id
            var uri = "Customer";
            request = CreateRequestToService(HttpMethod.Get, uri);
            response = await HttpClient.SendAsync(request);
            if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
            string jsonString = await response.Content.ReadAsStringAsync();
            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(jsonString);
            var customer = customers.FirstOrDefault(x => x.UserName == login.UserName);

            TempData["UserName"] = login.UserName;
            if (customer != null && login.UserName != "Employee") TempData["CustomerId"] = customer.Id;

            return RedirectToAction("Index", "Home");
        }

        // GET: Login/Logout
        public async Task<ActionResult> Logout()
        {
            if (!ModelState.IsValid)
            {
                return View("Error", new ErrorViewModel());
            }

            HttpRequestMessage request = CreateRequestToService(HttpMethod.Post, "Login/Logout");
            HttpResponseMessage response;

            try
            {
                response = await HttpClient.SendAsync(request);
            }
            catch (AggregateException)
            {
                return View("Error", new ErrorViewModel());
            }

            if (CheckIfErrorStatusCode(response) && (TempData["UserName"] == null || TempData["CustomerId"] == null))
            {
                PassCookiesToClient(response);
                return SelectErrorView(response);                
            }
            else
            {
                TempData["UserName"] = null;
                TempData["CustomerId"] = null;
                PassCookiesToClient(response);
                return RedirectToAction("Index", "Home");
            }
        }

        private bool PassCookiesToClient(HttpResponseMessage apiResponse)
        {
            if (apiResponse.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                string authValue = values.FirstOrDefault(x => x.StartsWith(s_CookieName));
                if (authValue != null)
                {
                    Response.Headers.Add("Set-Cookie", authValue);
                    return true;
                }
            }
            return false;
        }
    }
}