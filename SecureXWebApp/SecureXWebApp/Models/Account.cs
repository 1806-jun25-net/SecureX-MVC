using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class Account
    {
        [Required]
        [Display(Name = "Account ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }
        
        [Required]
        [Range(0.00, 1000000000.00)]
        public decimal Funds { get; set; }

        [Required]
        [Display(Name = "Customer ID")]
        public int CustomerId { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
