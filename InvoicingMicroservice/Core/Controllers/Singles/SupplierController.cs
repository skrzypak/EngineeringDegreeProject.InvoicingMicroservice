using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Interfaces.Services;
using InvoicingMicroservice.Core.Models.Dto.Supplier;
using InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InvoicingMicroservice.Core.Controllers.Singles
{
    [ApiController]
    [Route("/api/invoicing/1.0.0/{enterpriseId}/suppliers")]
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
        public ActionResult<object> GetSuppliers([FromRoute] int enterpriseId)
        {
            var supplierInformationsList = _supplierService.Get(enterpriseId);
            return Ok(supplierInformationsList);
        }

        [HttpGet("{supplierId}")]
        public ActionResult<object> GetSupplierById([FromRoute] int enterpriseId, [FromRoute] int supplierId)
        {
            var supplierInformations = _supplierService.GetById(enterpriseId, supplierId);
            return Ok(supplierInformations);
        }

        [HttpPost]
        public ActionResult CreateSupplier([FromRoute] int enterpriseId, [FromBody] SupplierCoreDto<SupplierContactPersonCoreDto> dto)
        {
            var supplierId = _supplierService.Create(enterpriseId, dto);
            return CreatedAtAction(nameof(GetSupplierById), new { enterpriseId = enterpriseId,  supplierId = supplierId }, null);
        }

        [HttpDelete("{supplierId}")]
        public ActionResult DeleteSupplier([FromRoute] int enterpriseId, [FromRoute] int supplierId)
        {
            _supplierService.Delete(enterpriseId, supplierId);
            return NoContent();
        }

        [HttpGet("{supplierId}/contacts/all")]
        public ActionResult<object> GetSuppContactPersons([FromRoute] int enterpriseId, [FromRoute] int supplierId)
        {
            var supplierInformations = _supplierService.GetContactPersons(enterpriseId, supplierId);
            return Ok(supplierInformations);
        }

        [HttpGet("{supplierId}/contacts/{suppContactPersonId}")]
        public ActionResult<object> GetSuppContactPersonById([FromRoute] int enterpriseId, [FromRoute] int supplierId, [FromRoute] int suppContactPersonId)
        {
            var supplierInformations = _supplierService.GetContactPersonById(enterpriseId, supplierId, suppContactPersonId);
            return Ok(supplierInformations);
        }

        [HttpPost("{supplierId}/contacts")]
        public ActionResult CreateContactPerson([FromRoute] int enterpriseId, [FromRoute] int supplierId, [FromBody] SupplierContactPersonCoreDto dto)
        {
            var contactPersonId = _supplierService.CreateContactPerson(enterpriseId, supplierId, dto);
            return CreatedAtAction(nameof(GetSuppContactPersonById), new { enterpriseId = enterpriseId, supplierId = supplierId, suppContactPersonId = contactPersonId }, null);
        }

        [HttpDelete("{id}/contacts/{suppContactPersonId}")]
        public ActionResult DeleteSuppContactPerson([FromRoute] int enterpriseId, [FromRoute] int id, [FromRoute] int suppContactPersonId)
        {
            _supplierService.DeleteContactPerson(enterpriseId, id, suppContactPersonId);
            return NoContent();
        }
    }
}
