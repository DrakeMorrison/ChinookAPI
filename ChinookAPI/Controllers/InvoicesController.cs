using ChinookAPI.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly DataStorage _storage;

        public InvoicesController(IConfiguration config)
        {
            _storage = new DataStorage(config);
        }

        [HttpGet("{id}")]
        public IActionResult GetAllByAgentId(int id)
        {
            return Ok(_storage.GetInvoiceByAgent(id));
        }

        [HttpGet]
        public IActionResult GetInvoiceData()
        {
            return Ok(_storage.GetInvoices());
        }
    }
}
