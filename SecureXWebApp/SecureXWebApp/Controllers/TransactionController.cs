using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SecureXWebApp.Models;

namespace SecureXWebApp.Controllers
{
    public class TransactionController : AServiceController
    {
        public TransactionController(HttpClient httpClient) : base(httpClient)
        { }

        //GET: Transaction
        //ELA async
        public async Task<IActionResult> Index()
        {
            var uri = "Transaction";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Transaction> transactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonString);
                List<Transaction> customerTransactions = new List<Transaction>();

                uri = "Account";
                request = CreateRequestToService(HttpMethod.Get, uri);
                try
                {
                    response = await HttpClient.SendAsync(request);
                    if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                    jsonString = await response.Content.ReadAsStringAsync();
                    List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(jsonString);
                    if (TempData["CustomerId"] != null)
                    {
                        var currentCustomerId = (int)TempData.Peek("CustomerId");
                        TempData.Keep("CustomerId");
                        accounts = accounts.FindAll(x => x.CustomerId == currentCustomerId);

                        if (accounts != null)
                        {
                            foreach (var account in accounts)
                            {
                                customerTransactions.AddRange(transactions.FindAll(x => x.AccountId == account.Id));
                            }

                            if (customerTransactions != null) return View(customerTransactions);
                        }
                    }
                    return View();
                }
                catch
                {
                    return View("Error", new ErrorViewModel());
                }
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: Transaction/Details/5
        //ELA async
        public async Task<IActionResult> Details(Transaction Transaction)
        {
            var uri = $"Transaction/{Transaction.Id}";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                Transaction transaction = JsonConvert.DeserializeObject<Transaction>(jsonString);
                return View(transaction);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // GET: Transaction/Create
        public async Task<ActionResult> Create()
        {
            var transactionVM = new TransactionViewModel();
            var uri = "Account";
            var request = CreateRequestToService(HttpMethod.Get, uri);
            try
            {
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);
                string jsonString = await response.Content.ReadAsStringAsync();
                List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(jsonString);
                if (TempData["CustomerId"] != null)
                {
                    var sCustomerId = (int)TempData.Peek("CustomerId");
                    TempData.Keep("CustomerId");
                    accounts = accounts.FindAll(x => x.CustomerId == sCustomerId);
                }
                // create list of account ids
                List<int> accountIds = new List<int>();
                foreach (var account in accounts)
                {
                    if (account.Status == "Active") accountIds.Add(account.Id);
                }
                transactionVM.AccountIds = accountIds;
                if (transactionVM.AccountIds.Count() == 0) return View("Error", new ErrorViewModel("No active accounts available to make a transaction."));
                return View(transactionVM);
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }

        // POST: Transaction/Create
        // ELA async
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionViewModel transactionVM)
        {
            if (!ModelState.IsValid)
            {
                return View(transactionVM);
            }
            try
            {
                if (transactionVM.Type == "Withdrawl")
                {
                    transactionVM.Transaction.TransactionAmount *= -1;
                }
                transactionVM.Transaction.AccountId = transactionVM.SelectedId;
                transactionVM.Transaction.DateOfTransaction = DateTime.Now;
                var uri = $"Transaction";
                var request = CreateRequestToService(HttpMethod.Post, uri, transactionVM.Transaction);
                var response = await HttpClient.SendAsync(request);
                if (CheckIfErrorStatusCode(response)) return SelectErrorView(response);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new ErrorViewModel());
            }
        }
    }
}