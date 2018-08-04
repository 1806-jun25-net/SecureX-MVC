using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class Transaction
    {
        [Required]
        [Display(Name = "Transaction ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Account ID")]
        public int AccountId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Recipient { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfTransaction { get; set; }

        [Required]
        [Range(1.00, 100000000.00)]
        [Display(Name = "Amount")]
        public decimal TransactionAmount { get; set; }
    }
}
