using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(User User)
        {
            if (!ModelState.IsValid)
            {
                return View(User);
            }
            try
            {
                var uri = $"User";
                
                var request = CreateRequestToService(HttpMethod.Post, uri, User);
                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                var login = new Login
                {
                    UserName = User.UserName,
                    Password = User.Password
                };

                HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Post, "api/Login/Register", login);

                HttpResponseMessage apiResponse;
                try
                {
                    apiResponse = await HttpClient.SendAsync(apiRequest);
                }
                catch
                {
                    return View("Error");
                }

                if (!apiResponse.IsSuccessStatusCode)
                {
                    return View("Error");
                }

                PassCookiesToClient(apiResponse);

            }
            catch
            {
                return View("Error");
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
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Post, "api/Login/Login", login);

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch (AggregateException)
            {
                return View("Error");
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    return View("AccessDenied");
                }
                ViewData["ErrorMessage"] = "Login credentials are invalid. Please try again or register if new.";
                return View();
            }

            PassCookiesToClient(apiResponse);

            return RedirectToAction("Index", "Home");
        }

        // GET: Login/Logout
        public async Task<ActionResult> Logout()
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Post, "api/Login/Logout");

            HttpResponseMessage apiResponse;

            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch (AggregateException)
            {
                return View("Error");
            }

            if (!apiResponse.IsSuccessStatusCode)
            {
                return View("Error");
            }

            PassCookiesToClient(apiResponse);

            return RedirectToAction("Index", "Home");
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