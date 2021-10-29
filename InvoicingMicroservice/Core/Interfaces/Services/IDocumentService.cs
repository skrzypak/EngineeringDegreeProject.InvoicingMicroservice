using System;
using System.Collections.Generic;
using InvoicingMicroservice.Core.Fluent.Entities;
using InvoicingMicroservice.Core.Fluent.Enums;
using InvoicingMicroservice.Core.Models.Dto.Document;
using InvoicingMicroservice.Core.Models.Dto.DocumentProduct;
using InvoicingMicroservice.Core.Models.Dto.DocumentType;

namespace InvoicingMicroservice.Core.Interfaces.Services
{
    public interface IDocumentService
    {
        public object GetDocuments(int?[] supplierIds, int?[] docTypeIds, DocumentState[] docStates, DateTime startDate, DateTime endDate);
        public object GetDocumentById(int docId);
        public int CreateDocument(DocumentRelationDto<int, DocumentToProductPushDto> dto);
        public int AddProductToDocument(int docId, DocumentToProductPushDto dto);
        public void ChangeDocumentState(int docId, DocumentState state);
        public void DeleteDocument(int docId);
        public void DeleteProductFromDocument(int docId, int docProdId);
        public int CreateDocumentType(DocumentTypeCoreDto dto);
        public void DeleteDocumentType(int docTypeId);
        public object GetDocumentTypeById(int docTypeId);
        public object GetDocumentsTypes();
    }
}
