using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class CreditCard
    {
        [Required]
        [Display(Name = "Credit Card ID")]
        public int Id { get; set; }

        [Required]
        [Range(0.00, 2000.00)]
        [Display(Name = "Available Credit")]
        public decimal CreditLimit { get; set; }

        [Required]
        [Range(0.00, 1000000.00)]
        [Display(Name = "Balance")]
        public decimal CurrentDebt { get; set; }

        [Required]
        [CreditCard]
        [Display(Name = "Card Number")]
        public long CreditCardNumber { get; set; }

        [Required]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Required]
        [Range(0.00, 2000.00)]
        [Display(Name = "Credit Line")]
        public decimal CreditLine { get; set; } = 2000.00m;

        [Required]
        public string Status { get; set; } = "Pending";

        public CreditCard()
        {
            Status = "Pending";
            CreditLine = 2000.00m;
            CreditLimit = CreditLine;
            CurrentDebt = 0.00m;
            CreditCardNumber = GenerateCardNumber();
        }

        private long GenerateCardNumber()
        {
            long cardNumber;

            Random random = new Random();
            string cardString = "";
            cardString += random.Next(1, 9).ToString();
            for (int i = 0; i < 15; i++)
            {
                cardString += random.Next(0, 9).ToString();
            }
            cardNumber = Convert.ToInt64(cardString);

            return cardNumber;
        }
    }
}
