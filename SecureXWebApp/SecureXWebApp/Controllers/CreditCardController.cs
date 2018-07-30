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
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error : CreditCard/Index");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                List<CreditCard> users = JsonConvert.DeserializeObject<List<CreditCard>>(jsonString);
                return View(users);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View("Error : CreditCard/Index");
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
                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : CreditCard/{CreditCard.Id}");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                CreditCard creditcard = JsonConvert.DeserializeObject<List<CreditCard>>(jsonString).FirstOrDefault(x => x.Id == CreditCard.Id);
                return View(creditcard);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return View($"Error : CreditCard/{CreditCard.Id}");
        }

        // GET: CreditCard/Create
        public ActionResult Create()
        {
            return View();
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
                var uri = $"CreditCard/{CreditCard.Id}";
                var request = CreateRequestToService(HttpMethod.Post, uri, CreditCard);

                var response = await HttpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View($"Error : CreditCard/{CreditCard.Id}");
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View($"Error : CreditCard/{CreditCard.Id}");
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
                    if (!response.IsSuccessStatusCode)
                    {
                        return View($"Error: CreditCard/{CreditCard.Id}");
                    }

                    return View("Credit card was deleted.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return View($"Error in CreditCard/{CreditCard.Id}");
            }
        }
    }
}