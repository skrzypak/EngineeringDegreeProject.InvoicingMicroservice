using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent;
using InvoicingMicroservice.Core.Fluent.Enums;
using InvoicingMicroservice.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroserviceController.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MicroserviceController : ControllerBase
    {
        private readonly ILogger<MicroserviceController> _logger;
        private readonly IMicroserviceService _microserviceService;

        public MicroserviceController(ILogger<MicroserviceController> logger, IMicroserviceService microserviceService)
        {
            _logger = logger;
            _microserviceService = microserviceService;
        }

        [HttpGet("suppliers-products-summary")]
        public ActionResult<object> GetSuppliersProductSummary(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] ICollection<int> suppliersIds,
            [FromQuery] ICollection<int> documentTypesIds,
            [FromQuery] ICollection<DocumentState> documentStates,
            [FromQuery] ICollection<int> productsIds
            )
        {
            var response = _microserviceService.GetSuppliersProductSummary(startDate, endDate, suppliersIds, documentTypesIds, documentStates, productsIds);
            return Ok(response);
        }

        [HttpGet("product-summary")]
        public ActionResult<object> GetProductsSummary(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] ICollection<int> documentTypesIds,
            [FromQuery] ICollection<DocumentState> documentStates,
            [FromQuery] ICollection<int> productsIds
            )
        {
            var response = _microserviceService.GetProductsSummary(startDate, endDate, documentTypesIds, documentStates, productsIds);
            return Ok(response);
        }
    }
}
