using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChinookAPI.DataAccess;
using Microsoft.Extensions.Configuration;
using ChinookAPI.Models;

namespace ChinookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DataStorage _storage;

        public EmployeeController (IConfiguration config)
        {
            _storage = new DataStorage(config);
        }

        public IActionResult Update(Employee employee)
        {
            return Ok(_storage.updateEmployee(employee));
        }
    }
}