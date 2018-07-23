using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountType { get; set; }
        //EA: should this be nullable?
        public decimal? Funds { get; set; }
    }
}
