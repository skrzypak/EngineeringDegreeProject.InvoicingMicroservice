using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent;
using InvoicingMicroservice.Core.Interfaces.Services;
using InvoicingMicroservice.Core.Models.Dto.Supplier;
using InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InvoicingMicroservice.Core.Controllers.Singles
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly ISupplierService _supplierService;

        public SupplierController(ILogger<SupplierController> logger, ISupplierService supplierService)
        {
            _logger = logger;
            _supplierService = supplierService;
        }

        [HttpGet("list")]
        public ActionResult<object> GetSuppliersList()
        {
            var supplierInformationsList = _supplierService.GetSuppliersList();
            return Ok(supplierInformationsList);
        }

        [HttpGet("{supplierId}")]
        public ActionResult<object> GetSupplierInformations([FromRoute] int supplierId)
        {
            var supplierInformations = _supplierService.GetSupplierInformations(supplierId);
            return Ok(supplierInformations);
        }

        [HttpPost]
        public ActionResult CreateSupplier([FromBody] SupplierCreateDto supplier)
        {
            var id = _supplierService.CreateSupplier(supplier);
            return NoContent();
        }

        [HttpDelete("{supplierId}")]
        public ActionResult DeleteSupplier([FromRoute] int supplierId)
        {
            _supplierService.DeleteSupplier(supplierId);
            return NoContent();
        }

        [HttpGet("{supplierId}/contacts")]
        public ActionResult<object> GetSupplierContacts([FromRoute] int supplierId)
        {
            var supplierInformations = _supplierService.GetSupplierContactPersons(supplierId);
            return Ok(supplierInformations);
        }

        [HttpPost("{id}/contacts/")]
        public ActionResult CreateSupplierContactPerson([FromRoute] int supplierId, [FromBody] SupplierContactPersonBasicDto supplierContactBasicDto)
        {
            var id = _supplierService.CreateSupplierContactPerson(supplierId, supplierContactBasicDto);
            return NoContent();
        }

        [HttpDelete("{supplierId}/contacts/{supplierContactPersonId}")]
        public ActionResult DeleteSupplierContactPerson([FromRoute] int supplierId, [FromRoute] int supplierContactPersonId)
        {
            _supplierService.DeleteSupplierContactPerson(supplierId, supplierContactPersonId);
            return NoContent();
        }
    }
}
