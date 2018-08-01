using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SecureXWebApp.Controllers
{
    public abstract class AServiceController : Controller
    {
        private static readonly Uri s_serviceUri = new Uri("http://securex-api.azurewebsites.net/api/");
        protected static readonly string s_CookieName = "SecureXApiAuth";

        protected HttpClient HttpClient { get; }

        public AServiceController(HttpClient httpClient)
        {
            // don't forget to register HttpClient as a singleton service in Startup.cs,
            // with the right HttpClientHandler
            HttpClient = httpClient;
        }

        protected HttpRequestMessage CreateRequestToService(HttpMethod method, string uri, object body = null)
        {
            var apiRequest = new HttpRequestMessage(method, new Uri(s_serviceUri, uri));

            if (body != null)
            {
                // set content header
                string jsonString = JsonConvert.SerializeObject(body);
                apiRequest.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            }

            string cookieValue = Request.Cookies[s_CookieName];

            if (cookieValue != null)
            {
                apiRequest.Headers.Add("Cookie", new CookieHeaderValue(s_CookieName, cookieValue).ToString());
            }

            return apiRequest;
        }

        protected bool CheckIfErrorStatusCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return true;
            }
            else return false;
        }

        protected IActionResult SelectErrorView(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Forbidden) return View("AccessDenied");
            return View("Error");
        }
    }
}