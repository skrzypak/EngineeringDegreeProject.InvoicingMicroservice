using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Enums;
using InvoicingMicroservice.Core.Interfaces.Services;
using InvoicingMicroservice.Core.Models.Dto.Document;
using InvoicingMicroservice.Core.Models.Dto.DocumentProduct;
using InvoicingMicroservice.Core.Models.Dto.DocumentType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InvoicingMicroservice.Core.Controllers.Singles
{
    [ApiController]
    [Route("/api/invoicing/1.0.0/{enterpriseId}/documents")]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly IDocumentService _documentService;

        public DocumentController(ILogger<DocumentController> logger, IDocumentService documentService)
        {
            _logger = logger;
            _documentService = documentService;
        }

        [HttpGet]
        public ActionResult<object> Get(
            [FromRoute] int enterpriseId,
            [FromQuery] int?[] suppliersId,
            [FromQuery] int?[] docTypeIds,
            [FromQuery] DocumentState[] docStates,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var response = _documentService.Get(enterpriseId, suppliersId, docTypeIds, docStates, startDate, endDate);
            return Ok(response);
        }

        [HttpGet("{docId}")]
        public ActionResult<object> GetById([FromRoute] int enterpriseId, [FromRoute] int docId)
        {
            var response = _documentService.GetById(enterpriseId, docId);
            return Ok(response);
        }

        [HttpGet("{docId}/products/{docProdId}")]
        public ActionResult GetProductById([FromRoute] int enterpriseId, [FromRoute] int docId, [FromRoute] int docProdId)
        {
            return NoContent();
        }

        [HttpGet("types")]
        public ActionResult<object> GetDocumentsTypes([FromRoute] int enterpriseId)
        {
            var response = _documentService.GetDocumentsTypes(enterpriseId);
            return Ok(response);
        }

        [HttpGet("types/{docTypeId}")]
        public ActionResult<object> GetDocumentTypeById([FromRoute] int enterpriseId, [FromRoute] int docTypeId)
        {
            var response = _documentService.GetDocumentTypeById(enterpriseId, docTypeId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromRoute] int enterpriseId, [FromBody] DocumentCoreDto<int, DocumentToProductCoreDto<int>, int> dto)
        {
            var docId = await _documentService.Create(enterpriseId, dto);
            return CreatedAtAction(nameof(GetById), new { enterpriseId = enterpriseId, docId = docId }, null);
        }

        [HttpPost("{docId}/products")]
        public async Task<ActionResult> AddProduct([FromRoute] int enterpriseId, [FromRoute] int docId, [FromBody] DocumentToProductCoreDto<int> dto)
        {
            var docProdId = await _documentService.AddProduct(enterpriseId, docId, dto);
            return CreatedAtAction(nameof(GetProductById), new { enterpriseId = enterpriseId, docId = docId, docProdId = docProdId }, null);
        }

        [HttpPost("types")]
        public ActionResult CreateDocumentType([FromRoute] int enterpriseId, [FromBody] DocumentTypeCoreDto dto)
        {
            var docTypeId = _documentService.CreateDocumentType(enterpriseId, dto);
            return CreatedAtAction(nameof(GetDocumentTypeById), new { enterpriseId = enterpriseId, docTypeId = docTypeId }, null);
        }

        [HttpPatch("{docId}/change/state")]
        public ActionResult ChangeDocumentState([FromRoute] int enterpriseId, [FromRoute] int docId, [FromQuery] DocumentState state)
        {
            _documentService.ChangeDocumentState(enterpriseId, docId, state);
            return NoContent();
        }

        [HttpDelete("{docId}")]
        public async Task<ActionResult> Delete([FromRoute] int enterpriseId, [FromRoute] int docId, [FromQuery] bool hardReset)
        {
            await _documentService.Delete(enterpriseId, docId, hardReset);
            return NoContent();
        }

        [HttpDelete("{docId}/products/{docProdId}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] int enterpriseId, [FromRoute] int docId, [FromRoute] int docProdId, [FromQuery] bool hardReset)
        {
            await _documentService.DeleteProduct(enterpriseId, docId, docProdId, hardReset);
            return NoContent();
        }

        [HttpPatch("{docId}/products/{docProdId}/transfer")]
        public async Task<ActionResult> TransferProduct([FromRoute] int enterpriseId, [FromRoute] int docId, [FromRoute] int docProdId)
        {
            await _documentService.TransferProduct(enterpriseId, docId, docProdId);
            return NoContent();
        }

        [HttpDelete("types/{docTypeId}")]
        public ActionResult DeleteDocumentType([FromRoute] int enterpriseId, [FromRoute] int docTypeId)
        {
            _documentService.DeleteDocumentType(enterpriseId, docTypeId);
            return NoContent();
        }

    }
}
