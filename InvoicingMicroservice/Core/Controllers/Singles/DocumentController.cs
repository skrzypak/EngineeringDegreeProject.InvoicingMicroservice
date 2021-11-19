using System;
using Authentication;
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
    [Route("/api/invoicing/1.0.0/documents")]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly IDocumentService _documentService;
        private readonly IHeaderContextService _headerContextService;

        public DocumentController(ILogger<DocumentController> logger, IDocumentService documentService, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _documentService = documentService;
            _headerContextService = headerContextService;
        }

        [HttpGet]
        public ActionResult<object> Get(
            [FromRoute] int espId,
            [FromQuery] int?[] suppliersId,
            [FromQuery] int?[] docTypeIds,
            [FromQuery] DocumentState[] docStates,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var response = _documentService.Get(espId, suppliersId, docTypeIds, docStates, startDate, endDate);
            return Ok(response);
        }

        [HttpGet("{docId}")]
        public ActionResult<object> GetById([FromQuery] int espId, [FromRoute] int docId)
        {
            var response = _documentService.GetById(espId, docId);
            return Ok(response);
        }

        [HttpGet("{docId}/products/{docProdId}")]
        public ActionResult GetProductById([FromQuery] int espId, [FromRoute] int docId, [FromRoute] int docProdId)
        {
            return NoContent();
        }

        [HttpGet("types")]
        public ActionResult<object> GetDocumentsTypes([FromQuery] int espId)
        {
            var response = _documentService.GetDocumentsTypes(espId);
            return Ok(response);
        }

        [HttpGet("types/{docTypeId}")]
        public ActionResult<object> GetDocumentTypeById([FromQuery] int espId, [FromRoute] int docTypeId)
        {
            var response = _documentService.GetDocumentTypeById(espId, docTypeId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromQuery] int espId, [FromBody] DocumentCoreDto<int, DocumentToProductCoreDto<int>, int> dto)
        {
            int eudId = _headerContextService.GetEudId();
            var docId = await _documentService.Create(espId, eudId, dto);
            return CreatedAtAction(nameof(GetById), new { espId = espId, docId = docId }, null);
        }

        [HttpPost("{docId}/products")]
        public async Task<ActionResult> AddProduct([FromQuery] int espId, [FromRoute] int docId, [FromBody] DocumentToProductCoreDto<int> dto)
        {
            int eudId = _headerContextService.GetEudId();
            var docProdId = await _documentService.AddProduct(espId, eudId, docId, dto);
            return CreatedAtAction(nameof(GetProductById), new { espId = espId, docId = docId, docProdId = docProdId }, null);
        }

        [HttpPost("types")]
        public ActionResult CreateDocumentType([FromQuery] int espId, [FromBody] DocumentTypeCoreDto dto)
        {
            int eudId = _headerContextService.GetEudId();
            var docTypeId = _documentService.CreateDocumentType(espId, eudId, dto);
            return CreatedAtAction(nameof(GetDocumentTypeById), new { espId = espId, docTypeId = docTypeId }, null);
        }

        [HttpPatch("{docId}/change/state")]
        public ActionResult ChangeDocumentState([FromQuery] int espId, [FromRoute] int docId, [FromQuery] DocumentState state)
        {
            int eudId = _headerContextService.GetEudId();
            _documentService.ChangeDocumentState(espId, eudId, docId, state);
            return NoContent();
        }

        [HttpDelete("{docId}")]
        public async Task<ActionResult> Delete([FromQuery] int espId, [FromRoute] int docId, [FromQuery] bool hardReset)
        {
            int eudId = _headerContextService.GetEudId();
            await _documentService.Delete(espId, eudId, docId, hardReset);
            return NoContent();
        }

        [HttpDelete("{docId}/products/{docProdId}")]
        public async Task<ActionResult> DeleteProduct([FromQuery] int espId, [FromRoute] int docId, [FromRoute] int docProdId, [FromQuery] bool hardReset)
        {
            int eudId = _headerContextService.GetEudId();
            await _documentService.DeleteProduct(espId, eudId, docId, docProdId, hardReset);
            return NoContent();
        }

        [HttpPatch("{docId}/products/{docProdId}/transfer")]
        public async Task<ActionResult> TransferProduct([FromQuery] int espId, [FromRoute] int docId, [FromRoute] int docProdId)
        {
            int eudId = _headerContextService.GetEudId();
            await _documentService.TransferProduct(espId, eudId, docId, docProdId);
            return NoContent();
        }

        [HttpDelete("types/{docTypeId}")]
        public ActionResult DeleteDocumentType([FromQuery] int espId, [FromRoute] int docTypeId)
        {
            int eudId = _headerContextService.GetEudId();
            _documentService.DeleteDocumentType(espId, eudId, docTypeId);
            return NoContent();
        }

    }
}
