using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChinookAPI.Models
{
    public class InvoiceInfo
    {
        public decimal Total { get; set; }
        public string CustomerName { get; set; }
        public string Country { get; set; }
        public string AgentName { get; set; }
    }
}
