using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureXWebApp.Models
{
    public class TransactionViewModel
    {
        public Transaction Transaction { get; set; }

        public string Type { get; set; }
        
        public int SelectedId { get; set; }
        public List<int> AccountIds { get; set; }
    }
}
