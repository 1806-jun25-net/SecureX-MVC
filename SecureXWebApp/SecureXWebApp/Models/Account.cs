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

        //EA: should this be nullable?
        [Required]
        [Range(1.00, 999999999.99)]
        public decimal? Funds { get; set; }
    }
}
