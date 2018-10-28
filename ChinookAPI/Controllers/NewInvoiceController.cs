using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChinookAPI.DataAccess;
using ChinookAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ChinookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewInvoiceController : ControllerBase
    {
        private readonly DataStorage _storage;

        public NewInvoiceController(IConfiguration config)
        {
            _storage = new DataStorage(config);
        }

        [HttpPost]
        //public IActionResult NewInvoice([FromBody] int customerId, [FromBody] string billingAddress)
        public IActionResult NewInvoice([FromBody] Invoice invoice)
        {
            return Ok(_storage.InsertInvoice(invoice));
        }
    }
}