using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Recipient { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
