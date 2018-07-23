using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class Bank
    {
        public int Id { get; set; }
        public decimal Reserves { get; set; }
        public string City { get; set; }
    }
}
