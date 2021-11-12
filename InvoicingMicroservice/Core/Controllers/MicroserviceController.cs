using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Enums;
using InvoicingMicroservice.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroserviceController.Core.Controllers
{
    [ApiController]
    [Route("/api/invoicing/1.0.0/{enterpriseId}/msv")]
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
            [FromRoute] int enterpriseId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] ICollection<int> suppliersIds,
            [FromQuery] ICollection<int> documentTypesIds,
            [FromQuery] ICollection<DocumentState> documentStates,
            [FromQuery] ICollection<int> productsIds
            )
        {
            var response = _microserviceService.GetSuppliersProductSummary(enterpriseId, startDate, endDate, suppliersIds, documentTypesIds, documentStates, productsIds);
            return Ok(response);
        }

        [HttpGet("products-summary")]
        public ActionResult<object> GetProductsSummary(
            [FromRoute] int enterpriseId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] ICollection<int> documentTypesIds,
            [FromQuery] ICollection<DocumentState> documentStates,
            [FromQuery] ICollection<int> productsIds
            )
        {
            var response = _microserviceService.GetProductsSummary(enterpriseId, startDate, endDate, documentTypesIds, documentStates, productsIds);
            return Ok(response);
        }
    }
}
