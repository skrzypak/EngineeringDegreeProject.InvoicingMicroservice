using System;
using System.Collections.Generic;
using InvoicingMicroservice.Core.Models.Dto.Document;
using InvoicingMicroservice.Core.Models.Dto.DocumentProduct;

namespace InvoicingMicroservice.Core.Interfaces.Services
{
    public interface IDocumentService
    {
        public object GetUnsettledDocuments(int[] suppliersId, DateTime startDate, DateTime endDate);
        public object GetSettledDocuments(int[] suppliersId, DateTime startDate, DateTime endDate);
        public object GetDocumentById(int id);
        public int CreateDocument(DocumentRelationDto<int, int, DocumentProductCoreDto> dto);
        public int AddProductToDocument(DocumentProductRelationDto<int, int> dto);
        public void ChangeDocumentState(int docId, int state);
        public void DeleteDocument(int docId);
        public void DeleteProductFromDocument(int docId, int docToProductId);

    }
}
