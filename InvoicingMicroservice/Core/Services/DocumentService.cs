using System;
using System.Linq;
using AutoMapper;
using InvoicingMicroservice.Core.Exceptions;
using InvoicingMicroservice.Core.Fluent;
using InvoicingMicroservice.Core.Fluent.Entities;
using InvoicingMicroservice.Core.Fluent.Enums;
using InvoicingMicroservice.Core.Interfaces.Services;
using InvoicingMicroservice.Core.Models.Dto.Document;
using InvoicingMicroservice.Core.Models.Dto.DocumentProduct;
using InvoicingMicroservice.Core.Models.Dto.DocumentType;
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

        public int AddProduct(int docId, DocumentToProductCoreDto<int> dto)
        {
            var model = _mapper.Map<DocumentToProductCoreDto<int>, DocumentToProduct>(dto);

            model.DocumentId = docId;

            _context.DocumentToProducts.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void ChangeDocumentState(int docId, DocumentState state)
        {
            var model = _context.Documents
                .FirstOrDefault(d => d.Id == docId);

            if(model is null)
            {
                throw new NotFoundException($"Document with id {docId} NOT FOUND");
            }

            model.State = state;

            _context.SaveChanges();
        }

        public int Create(DocumentCoreDto<int, DocumentToProductCoreDto<int>, int> dto)
        {
            var model = _mapper.Map<DocumentCoreDto<int, DocumentToProductCoreDto<int>, int>, Document>(dto);

            _context.Documents.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public int CreateDocumentType(DocumentTypeCoreDto dto)
        {
            var model = _mapper.Map<DocumentTypeCoreDto, DocumentType>(dto);

            _context.DocumentsTypes.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int docId)
        {
            var model = new Document() { Id = docId };

            _context.Documents.Attach(model);
            _context.Documents.Remove(model);
            _context.SaveChanges();
        }

        public void DeleteDocumentType(int docTypeId)
        {
            var model = new DocumentType() { Id = docTypeId };

            _context.DocumentsTypes.Attach(model);
            _context.DocumentsTypes.Remove(model);
            _context.SaveChanges();
        }

        public void DeleteProduct(int docId, int docProdId)
        {
            var model = new DocumentToProduct() { Id = docProdId, DocumentId = docId };

            _context.DocumentToProducts.Attach(model);
            _context.DocumentToProducts.Remove(model);
            _context.SaveChanges();
        }

        public object GetById(int docId)
        {
            var dto = _context.Documents
                .AsNoTracking()
                .Include(d => d.Supplier)
                .Include(d => d.DocumentType)
                .Include(d => d.DocumentsToProducts)
                    .ThenInclude(dtp => dtp.Product)
                .Where(d => d.Id == docId)
                .Select(d => new
                {
                    d.Id,
                    d.SupplierId,
                    d.Signature,
                    d.Number,
                    d.Description,
                    d.Date,
                    d.DocumentType,
                    DocumentProducts = d.DocumentsToProducts.Select(dtp => new
                    {
                        dtp.Id,
                        dtp.Quantity,
                        dtp.UnitNetPrice,
                        dtp.PercentageVat,
                        dtp.NetValue,
                        dtp.VatValue,
                        dtp.GrossValue,
                        dtp.Transfered,
                        Product = new
                        {
                            dtp.Product.Id,
                            dtp.Product.Code,
                            dtp.Product.Name,
                            dtp.Product.Unit,
                            dtp.Product.Description
                        }
                    })
                })
                .FirstOrDefault();

            if(dto is null)
            {
                throw new NotFoundException($"Document with id {dto.Id} NOT FOUND");
            }

            return dto;
        }

        public object Get(int?[] supplierId, int?[] docTypeIds, DocumentState[] docStates, DateTime startDate, DateTime endDate)
        {
            var iquery = _context.Documents
               .AsNoTracking()
               .Include(d => d.Supplier)
               .Include(d => d.DocumentType);

            IQueryable<Document> query = iquery;

            if (supplierId is not null && supplierId.Length > 0)
            {
                query = query.Where(d => supplierId.Contains(d.SupplierId));
            }

            if (docTypeIds is not null && docTypeIds.Length > 0)
            {
                query = query.Where(d => docTypeIds.Contains(d.DocumentTypeId));
            }

            if (docStates is not null && docStates.Length > 0)
            {
                query = query.Where(d => docStates.Contains(d.State));
            }

            var dtos = query.Where(d => d.Date >= startDate && d.Date <= endDate)
                .Select(d => new
                    {
                        d.Id,
                        d.SupplierId,
                        d.Signature,
                        d.Number,
                        d.DocumentType,
                        d.State,
                        d.Date,
                        d.Description,
                    })
                .ToList()
                .OrderByDescending(dx => dx.Date);

            if (dtos is null)
            {
                throw new NotFoundException("Not found any documents");
            }

            return dtos;
        }

        public object GetDocumentsTypes()
        {
            var dto = _context.DocumentsTypes
                .AsNoTracking()
                .Select(d => new
                {
                    d.Id,
                    d.Code,
                    d.Name,
                    d.Description
                })
                .AsEnumerable();

            if (dto is null)
            {
                throw new NotFoundException($"NOT FOUND any document types");
            }

            return dto;
        }

        public object GetDocumentTypeById(int docTypeId)
        {
            var dto = _context.DocumentsTypes
                .AsNoTracking()
                .Where(d => d.Id == docTypeId)
                .Select(d => new
                {
                    d.Id,
                    d.Code,
                    d.Name,
                    d.Description
                })
                .FirstOrDefault();

            if (dto is null)
            {
                throw new NotFoundException($"Document type with id {dto.Id} NOT FOUND");
            }

            return dto;
        }
    }
}
