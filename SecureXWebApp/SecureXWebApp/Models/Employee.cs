using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class Employee
    {
        [Required]
        [Display(Name = "Employee ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Bank ID")]
        public int BankId { get; set; }
    }
}
