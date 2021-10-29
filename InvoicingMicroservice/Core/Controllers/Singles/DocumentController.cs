using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Interfaces.Services;
using InvoicingMicroservice.Core.Models.Dto.Document;
using InvoicingMicroservice.Core.Models.Dto.DocumentProduct;
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

        [HttpGet("unsettled/{suppliersId}&{startDate}&{endDate}")]
        public ActionResult<object> GetUnsettledDocuments(
            [FromRoute] int[] suppliersId,
            [FromRoute] DateTime startDate,
            [FromRoute] DateTime endDate)
        {
            var response = _documentService.GetUnsettledDocuments(suppliersId, startDate, endDate);
            return Ok(response);
        }

        [HttpGet("settled/{suppliersId}&{startDate}&{endDate}")]
        public ActionResult<object> GetSettledDocuments(
            [FromRoute] int[] suppliersId,
            [FromRoute] DateTime startDate,
            [FromRoute] DateTime endDate)
        {
            var response = _documentService.GetSettledDocuments(suppliersId, startDate, endDate);
            return Ok(response);
        }

        [HttpGet("all")]
        public ActionResult<object> GetDocumentById([FromRoute] int id)
        {
            var response = _documentService.GetDocumentById(id);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult CreateDocument([FromBody] DocumentRelationDto<int, int, DocumentProductCoreDto> dto)
        {
            _documentService.CreateDocument(dto);
            return NoContent();
        }

        [HttpPost("product")]
        public ActionResult AddProductToDocument([FromBody] DocumentProductRelationDto<int, int> dto)
        {
            _documentService.AddProductToDocument(dto);
            return NoContent();
        }

        [HttpPatch("{docId}&{state}")]
        public ActionResult ChangeDocumentState([FromRoute] int docId, [FromRoute] int state)
        {
            _documentService.ChangeDocumentState(docId, state);
            return NoContent();
        }

        [HttpDelete("{docId}")]
        public ActionResult DeleteDocument([FromRoute] int docId)
        {
            _documentService.DeleteDocument(docId);
            return NoContent();
        }

        [HttpDelete("{docId}&{docToProductId}")]
        public ActionResult DeleteProductFromDocument([FromRoute] int docId, [FromRoute] int docToProductId)
        {
            _documentService.DeleteProductFromDocument(docId, docToProductId);
            return NoContent();
        }

    }
}
