using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public string City { get; set; }
    }
}
