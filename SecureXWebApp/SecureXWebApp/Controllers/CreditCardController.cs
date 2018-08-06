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
    public class CreditCardController : AServiceController
    {
        public CreditCardController(HttpClient httpClient) : base(httpClient)
        { }

        //GET: CreditCard
        //ELA async
        public async Task<IActionResult> Index()
        {
            var uri = "CreditCard";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                List<CreditCard> creditcards = JsonConvert.DeserializeObject<List<CreditCard>>(jsonString);
                if (TempData["CustomerId"] != null)
                {
                    var sCustomerId = (int)TempData.Peek("CustomerId");
                    TempData.Keep("CustomerId");
                    creditcards = creditcards.FindAll(x => x.CustomerId == sCustomerId);
                }
                if (creditcards != null) return View(creditcards);
                else return View();
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }


        // GET: CreditCard/Details/5
        //ELA async
        public async Task<IActionResult> Details(CreditCard CreditCard)
        {
            var uri = $"CreditCard/{CreditCard.Id}";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                CreditCard creditcard = JsonConvert.DeserializeObject<CreditCard>(jsonString);
                return View(creditcard);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: CreditCard/Create
        public ActionResult Create()
        {
            var card = new CreditCard();
            card.CustomerId = (int)TempData.Peek("CustomerId");
            TempData.Keep("CustomerId");
            return View(card);
        }

        // POST: CreditCard/Create
        //ELA async
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreditCard CreditCard)
        {
            if (!ModelState.IsValid)
            {
                return View(CreditCard);
            }
            try
            {
                var uri = $"CreditCard";
                var request = CreateRequestToService(HttpMethod.Post, uri, CreditCard);
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: CreditCard/Freeze/5
        public async Task<IActionResult> Freeze(int id)
        {
            var uri = $"CreditCard/{id}";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                CreditCard creditCard = JsonConvert.DeserializeObject<CreditCard>(jsonString);
                return View(creditCard);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // PUT: CreditCard/Freeze/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Freeze(CreditCard CreditCard)
        {
            CreditCard.Status = "Frozen";
            var uri = $"CreditCard/{CreditCard.Id}";
            var request = CreateRequestToService(HttpMethod.Put, uri, CreditCard);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                var creditCard = JsonConvert.DeserializeObject<CreditCard>(jsonString);

                return View("Details", CreditCard);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: CreditCard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CreditCard/Delete/5
        //ELA async
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CreditCard CreditCard)
        {
            {
                var uri = $"CreditCard/{CreditCard.Id}";
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