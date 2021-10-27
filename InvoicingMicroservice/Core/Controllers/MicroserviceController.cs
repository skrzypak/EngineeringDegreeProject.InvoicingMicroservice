using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroserviceController.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MicroserviceController : ControllerBase
    {
        private readonly ILogger<MicroserviceController> _logger;

        public MicroserviceController(ILogger<MicroserviceController> logger)
        {
            _logger = logger;
        }
    }
}
