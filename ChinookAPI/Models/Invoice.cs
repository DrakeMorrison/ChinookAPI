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
        public string BillingCity { get; internal set; }
        public string BillingState { get; internal set; }
        public string BillingCountry { get; internal set; }
        public string BillingPostalCode { get; internal set; }
        public decimal Total { get; internal set; }
    }
}
