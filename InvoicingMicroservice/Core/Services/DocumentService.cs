using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InvoicingMicroservice.Core.Exceptions;
using InvoicingMicroservice.Core.Fluent;
using InvoicingMicroservice.Core.Fluent.Entities;
using InvoicingMicroservice.Core.Interfaces.Services;
using InvoicingMicroservice.Core.Models.Dto.Document;
using InvoicingMicroservice.Core.Models.Dto.DocumentProduct;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvoicingMicroservice.Core.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ILogger<DocumentService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;

        public DocumentService(
            ILogger<DocumentService> logger,
            MicroserviceContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public int AddProductToDocument(DocumentProductRelationDto<int, int> dto)
        {
            throw new NotImplementedException();
        }

        public void ChangeDocumentState(int docId, int state)
        {
            throw new NotImplementedException();
        }

        public int CreateDocument(DocumentRelationDto<int, int, DocumentProductCoreDto> dto)
        {
            throw new NotImplementedException();
        }

        public void DeleteDocument(int docId)
        {
            throw new NotImplementedException();
        }

        public void DeleteProductFromDocument(int docId, int docToProductId)
        {
            throw new NotImplementedException();
        }

        public object GetDocumentById(int id)
        {
            throw new NotImplementedException();
        }

        public object GetSettledDocuments(int[] suppliersId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public object GetUnsettledDocuments(int[] suppliersId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
