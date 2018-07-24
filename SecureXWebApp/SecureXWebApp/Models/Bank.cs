using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class Bank
    {
        [Required]
        [Display(Name = "Bank ID")]
        public int Id { get; set; }

        [Required]
        [Range(1.00, 999999999.99)]
        public decimal Reserves { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string City { get; set; }
    }
}
