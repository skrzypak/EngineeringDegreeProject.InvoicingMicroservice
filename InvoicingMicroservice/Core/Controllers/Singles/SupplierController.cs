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

        [HttpGet("all")]
        public ActionResult<object> GetSuppliers()
        {
            var supplierInformationsList = _supplierService.GetSuppliers();
            return Ok(supplierInformationsList);
        }

        [HttpGet("{supplierId}")]
        public ActionResult<object> GetSupplierById([FromRoute] int supplierId)
        {
            var supplierInformations = _supplierService.GetSupplierById(supplierId);
            return Ok(supplierInformations);
        }

        [HttpPost]
        public ActionResult CreateSupplier([FromBody] SupplierRelationDto<SupplierContactPersonCoreDto> dto)
        {
            var supplierId = _supplierService.CreateSupplier(dto);
            return CreatedAtAction(nameof(GetSupplierById), new { supplierId = supplierId }, null);
        }

        [HttpDelete("{supplierId}")]
        public ActionResult DeleteSupplier([FromRoute] int supplierId)
        {
            _supplierService.DeleteSupplier(supplierId);
            return NoContent();
        }

        [HttpGet("{supplierId}/contact/all")]
        public ActionResult<object> GetSuppContactPersons([FromRoute] int supplierId)
        {
            var supplierInformations = _supplierService.GetSuppContactPersons(supplierId);
            return Ok(supplierInformations);
        }

        [HttpGet("{supplierId}/contact/{suppContactPersonId}")]
        public ActionResult<object> GetSuppContactPersonById([FromRoute] int supplierId, [FromRoute] int suppContactPersonId)
        {
            var supplierInformations = _supplierService.GetSuppContactPersonById(supplierId, suppContactPersonId);
            return Ok(supplierInformations);
        }

        [HttpPost("{supplierId}/contact")]
        public ActionResult CreateContactPerson([FromRoute] int supplierId, [FromBody] SupplierContactPersonRelationDto<int> dto)
        {
            var contactPersonId = _supplierService.CreateSuppContactPerson(supplierId, dto);
            return CreatedAtAction(nameof(GetSuppContactPersonById), new { supplierId = supplierId, suppContactPersonId = contactPersonId }, null);
        }

        [HttpDelete("{id}/contact/{suppContactPersonId}")]
        public ActionResult DeleteSuppContactPerson([FromRoute] int id, [FromRoute] int suppContactPersonId)
        {
            _supplierService.DeleteSuppContactPerson(id, suppContactPersonId);
            return NoContent();
        }
    }
}
