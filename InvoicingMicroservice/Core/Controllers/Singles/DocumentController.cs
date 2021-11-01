using System;
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
    [Route("[controller]")]
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
            [FromQuery] int?[] suppliersId,
            [FromQuery] int?[] docTypeIds,
            [FromQuery] DocumentState[] docStates,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var response = _documentService.Get(suppliersId, docTypeIds, docStates, startDate, endDate);
            return Ok(response);
        }

        [HttpGet("{docId}")]
        public ActionResult<object> GetById([FromRoute] int docId)
        {
            var response = _documentService.GetById(docId);
            return Ok(response);
        }

        [HttpGet("{docId}/product/{docProdId}")]
        public ActionResult GetProductById([FromRoute] int docId, [FromRoute] int docProdId)
        {
            return NoContent();
        }

        [HttpGet("type")]
        public ActionResult<object> GetDocumentsTypes()
        {
            var response = _documentService.GetDocumentsTypes();
            return Ok(response);
        }

        [HttpGet("type/{docTypeId}")]
        public ActionResult<object> GetDocumentTypeById([FromRoute] int docTypeId)
        {
            var response = _documentService.GetDocumentTypeById(docTypeId);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult Create([FromBody] DocumentCoreDto<int, DocumentToProductCoreDto<int>, int> dto)
        {
            var docId = _documentService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { docId = docId }, null);
        }

        [HttpPost("{docId}/product")]
        public ActionResult AddProduct([FromRoute] int docId, [FromBody] DocumentToProductCoreDto<int> dto)
        {
            var docProdId = _documentService.AddProduct(docId, dto);
            return CreatedAtAction(nameof(GetProductById), new { docId = docId, docProdId = docProdId }, null);
        }

        [HttpPost("type")]
        public ActionResult CreateDocumentType([FromBody] DocumentTypeCoreDto dto)
        {
            var docTypeId = _documentService.CreateDocumentType(dto);
            return CreatedAtAction(nameof(GetDocumentTypeById), new { docTypeId = docTypeId }, null);
        }

        [HttpPatch("{docId}/change/state")]
        public ActionResult ChangeDocumentState([FromRoute] int docId, [FromQuery] DocumentState state)
        {
            _documentService.ChangeDocumentState(docId, state);
            return NoContent();
        }

        [HttpDelete("{docId}")]
        public ActionResult Delete([FromRoute] int docId)
        {
            _documentService.Delete(docId);
            return NoContent();
        }

        [HttpDelete("{docId}/product/{docProdId}")]
        public ActionResult DeleteProduct([FromRoute] int docId, [FromRoute] int docProdId)
        {
            _documentService.DeleteProduct(docId, docProdId);
            return NoContent();
        }

        [HttpDelete("type/{docTypeId}")]
        public ActionResult DeleteDocumentType([FromRoute] int docTypeId)
        {
            _documentService.DeleteDocumentType(docTypeId);
            return NoContent();
        }

    }
}
