using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class RegisterViewModel
    {
        public Login Login { get; set; }
        public User User { get; set; }
        public Customer Customer { get; set; }        
    }
}
