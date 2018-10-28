using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChinookAPI.Models
{
    public class Invoice
    {
        public int CustomerId { get; set; }
        public string BillingAddress { get; set; }
        public object InvoiceDate { get; set; }
        public string BillingCity { get;  set; }
        public string BillingState { get;  set; }
        public string BillingCountry { get;  set; }
        public string BillingPostalCode { get;  set; }
        public decimal Total { get;  set; }
        public string AgentName { get;  set; }
    }
}
