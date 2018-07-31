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
        [Display(Name = "Credit Limit")]
        public decimal CreditLimit { get; set; }

        [Required]
        [Range(0.00, 1000000.00)]
        [Display(Name = "Current Debt")]
        public decimal CurrentDebt { get; set; }

        [Required]
        [CreditCard]
        [Display(Name = "Credit Card Number")]
        public long CreditCardNumber { get; set; }

        [Required]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }
    }
}
