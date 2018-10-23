using ChinookAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChinookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceStorage _storage;

        public InvoicesController()
        {
            _storage = new InvoiceStorage();
        }

        [HttpGet("{id}")]
        public IActionResult GetByAgentId(int id)
        {
            return Ok(_storage.GetInvoice(id));
        }
    }
}
