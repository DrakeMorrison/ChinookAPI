using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChinookAPI.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChinookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceLineController : ControllerBase
    {
        private readonly InvoiceStorage _storage;

        public InvoiceLineController()
        {
            _storage = new InvoiceStorage();
        }

        [HttpGet("{invoiceId}")]
        public IActionResult ItemPerInvoice(int invoiceId)
        {
            var value = _storage.LineItemsPerInvoice(invoiceId);
            if(value == -1)
            {
                return BadRequest();
            }
            return Ok(value);
        }
    }
}