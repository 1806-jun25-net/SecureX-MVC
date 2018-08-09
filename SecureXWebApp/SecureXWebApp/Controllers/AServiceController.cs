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
using SecureXWebApp.Models;

namespace SecureXWebApp.Controllers
{
    public abstract class AServiceController : Controller
    {
        private static readonly Uri s_serviceUri = Settings.ServiceUri;
        protected static readonly string s_CookieName = "SecureXApiAuth";

        protected HttpClient HttpClient { get; }
        protected static Settings Settings { get; set; }

        public AServiceController(HttpClient httpClient, Settings settings)
        {
            HttpClient = httpClient;
            Settings = settings;
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

        protected ActionResult SelectErrorView(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest) return View("Error", new ErrorViewModel("400: Bad Request","Sorry, there was something wrong with your request."));
            else if (response.StatusCode == HttpStatusCode.Unauthorized) return View("Error", new ErrorViewModel("401: Unauthorized", "Access denied. Try logging in first."));
            else if (response.StatusCode == HttpStatusCode.Forbidden) return View("Error", new ErrorViewModel("403: Forbidden","Access denied. That feature is not available to you."));
            else if (response.StatusCode == HttpStatusCode.NotFound) return View("Error", new ErrorViewModel("404: Not Found","Sorry, that page doesn't exist."));
            else if (response.StatusCode == HttpStatusCode.InternalServerError) return View("Error", new ErrorViewModel("500: Internal Server Error", "Sorry, something went wrong with the internal server."));
            else if (response.StatusCode == HttpStatusCode.BadGateway) return View("Error", new ErrorViewModel("502: Bad Gateway", "Sorry, something went wrong with the gateway server."));
            return View("Error", new ErrorViewModel());
        }
    }
}