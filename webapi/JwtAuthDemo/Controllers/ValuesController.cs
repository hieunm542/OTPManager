using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JwtAuthDemo.Infrastructure;
using Newtonsoft.Json.Linq;
using JwtAuthDemo.Models;

namespace JwtAuthDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            var userName = User.Identity?.Name;
            _logger.LogInformation($"User [{userName}] is viewing values.");
            return new[] { "value1", "value2" };
        }
    }
}
