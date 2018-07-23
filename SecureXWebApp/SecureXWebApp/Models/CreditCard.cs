using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CurrentDebt { get; set; }
        public int CreditCardNumber { get; set; }
        public int CustomerId { get; set; }
    }
}
