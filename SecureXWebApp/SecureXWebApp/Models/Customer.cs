using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class Customer
    {
        [Required]
        [Display(Name = "Customer ID")]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public long PhoneNumber { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string City { get; set; }
    }
}
