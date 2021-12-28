using Authentication;
using InvoicingMicroservice.Core.Interfaces.Services;
using InvoicingMicroservice.Core.Models.Dto.Supplier;
using InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InvoicingMicroservice.Core.Controllers.Singles
{
    [ApiController]
    [Route("/api/purchases/1.0.0/suppliers")]
    public class SupplierController : ControllerBase
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly ISupplierService _supplierService;
        private readonly IHeaderContextService _headerContextService;

        public SupplierController(ILogger<SupplierController> logger, ISupplierService supplierService, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _supplierService = supplierService;
            _headerContextService = headerContextService;
        }

        [HttpGet]
        public ActionResult<object> GetSuppliers([FromQuery] int espId)
        {
            var supplierInformationsList = _supplierService.Get(espId);
            return Ok(supplierInformationsList);
        }

        [HttpGet("{supplierId}")]
        public ActionResult<object> GetSupplierById([FromQuery] int espId, [FromRoute] int supplierId)
        {
            var supplierInformations = _supplierService.GetById(espId, supplierId);
            return Ok(supplierInformations);
        }

        [HttpPost]
        public ActionResult CreateSupplier([FromQuery] int espId, [FromBody] SupplierCoreDto<SupplierContactPersonCoreDto> dto)
        {
            int eudId = _headerContextService.GetEudId();
            var supplierId = _supplierService.Create(espId, eudId, dto);
            return CreatedAtAction(nameof(GetSupplierById), new { espId = espId,  supplierId = supplierId }, null);
        }

        [HttpPatch("{supplierId}")]
        public ActionResult UpdateSupplier([FromQuery] int espId, [FromRoute] int supplierId, [FromBody] SupplierDto<SupplierContactPersonDto<int>> dto)
        {
            int eudId = _headerContextService.GetEudId();
            _supplierService.UpdateSupplier(espId, eudId, supplierId, dto);
            return NoContent();
        }

        [HttpDelete("{supplierId}")]
        public ActionResult DeleteSupplier([FromQuery] int espId, [FromRoute] int supplierId)
        {
            int eudId = _headerContextService.GetEudId();
            _supplierService.Delete(espId, eudId, supplierId);
            return NoContent();
        }

        [HttpGet("{supplierId}/contacts/all")]
        public ActionResult<object> GetSuppContactPersons([FromQuery] int espId, [FromRoute] int supplierId)
        {
            var supplierInformations = _supplierService.GetContactPersons(espId, supplierId);
            return Ok(supplierInformations);
        }

        [HttpGet("{supplierId}/contacts/{suppContactPersonId}")]
        public ActionResult<object> GetSuppContactPersonById([FromQuery] int espId, [FromRoute] int supplierId, [FromRoute] int suppContactPersonId)
        {
            var supplierInformations = _supplierService.GetContactPersonById(espId, supplierId, suppContactPersonId);
            return Ok(supplierInformations);
        }

        [HttpPost("{supplierId}/contacts")]
        public ActionResult CreateContactPerson([FromQuery] int espId, [FromRoute] int supplierId, [FromBody] SupplierContactPersonCoreDto dto)
        {
            int eudId = _headerContextService.GetEudId();
            var contactPersonId = _supplierService.CreateContactPerson(espId, eudId, supplierId, dto);
            return CreatedAtAction(nameof(GetSuppContactPersonById), new { espId = espId, supplierId = supplierId, suppContactPersonId = contactPersonId }, contactPersonId);
        }

        [HttpPut("{supplierId}/contacts")]
        public ActionResult UpdateContactPerson([FromQuery] int espId, [FromRoute] int supplierId, [FromBody] SupplierContactPersonDto<int> dto)
        {
            int eudId = _headerContextService.GetEudId();
           _supplierService.UpdateContactPerson(espId, eudId, supplierId, dto);
            return NoContent();
        }

        [HttpDelete("{supplierId}/contacts/{suppContactPersonId}")]
        public ActionResult DeleteSuppContactPerson([FromQuery] int espId, [FromRoute] int supplierId, [FromRoute] int suppContactPersonId)
        {
            int eudId = _headerContextService.GetEudId();
            _supplierService.DeleteContactPerson(espId, eudId, supplierId, suppContactPersonId);
            return NoContent();
        }
    }
}
